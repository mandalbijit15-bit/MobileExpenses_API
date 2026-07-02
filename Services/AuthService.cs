using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MobileExpenses_API.DTOs.RequestDTO;
using MobileExpenses_API.DTOs.ResponseDTO;
using MobileExpenses_API.Interfaces;
using MobileExpenses_API.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;

namespace MobileExpenses_API.Services
{
    public class AuthService : IAuthService
    {
        readonly MobileExpensesDbContext _context;
        readonly IConfiguration _configuration;
        public AuthService(MobileExpensesDbContext context, IConfiguration configuration, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _configuration = configuration;
        }

        public string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];

            using var rng = RandomNumberGenerator.Create();

            rng.GetBytes(randomBytes);

            return Convert.ToBase64String(randomBytes);
        }

        public string GnerateJWTTokenAsync(User user, List<string> roles)
        {
               var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Userid.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    Convert.ToDouble(_configuration["Jwt:ExpiryMinutes"])),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<LoginResultDTO> LoginAsync(LoginDTO loginDTO)
        {
            var user = await _context.Users
            .Include(u => u.Roles)
            .FirstOrDefaultAsync(u =>
             u.Username == loginDTO.Username);

            if (user == null)
            {
                throw new Exception("Invalid username or password.");
            }

            var validPassword = BCrypt.Net.BCrypt.Verify(
                loginDTO.Password,
                user.Passwordhash);

            if (!validPassword)
            {
                throw new Exception("Invalid username or password.");
            }

            var roles = user.Roles
                .Select(r => r.Rolename)
                .ToList();

            var token = GnerateJWTTokenAsync(user, roles);

            var refreshToken = GenerateRefreshToken();

            var refreshTokenEntity = new Refreshtoken
            {
                Userid = user.Userid,
                Token = refreshToken,
                Expiresat = DateTime.UtcNow.AddMinutes(
                    Convert.ToDouble(_configuration["Refresh:ExpiryMinutes"])),
            };

            _context.Refreshtokens.Add(refreshTokenEntity);

            await _context.SaveChangesAsync();


            return new LoginResultDTO
            {
                RefreshToken = refreshToken,
                AuthResponse = new AuthResponseDto
                {
                    UserId = user.Userid,
                    Username = user.Username,
                    Token = token,
                    Roles = user.Roles.Select(x => x.Rolename).ToList(),
                }
            };
         }

        public async Task<string> Logout(string refreshToken)
        {
                var token = await _context.Refreshtokens
                        .Include(x => x.User)
                        .FirstOrDefaultAsync(x => x.Token == refreshToken);          

            // grab all the refresh tokens for the user and revoke them

            if (token != null)
            {
                _context.Refreshtokens.Where(x => x.Userid == token.Userid).ToList().ForEach(t => t.Isrevoked = true);
                await _context.SaveChangesAsync();
                
                return  "All Refresh Tokens for" + token.User.Username + "are revoked ";
            }

            return "Refresh Token not found for user";
        }

        public async Task<Refreshtoken> Refresh(string refreshToken)
        {
            var storedToken = await _context.Refreshtokens
                 .Include(x => x.User)
                 .FirstOrDefaultAsync(x => x.Token == refreshToken);

            return storedToken ?? throw new Exception("refresh token not found.");
        }

        public async Task<LoginResultDTO> RegisterAsync( RegisterDTO registerDTO)
        {
            var existingUser = await _context.Users
                   .FirstOrDefaultAsync(u =>
            u.Username == registerDTO.Username ||
            u.Email == registerDTO.Email);

            if (existingUser != null)
            {
                throw new Exception("User already exists.");
            }

            var user = new User
            {
                Username = registerDTO.Username,
                Email = registerDTO.Email,
                Passwordhash = BCrypt.Net.BCrypt.HashPassword(registerDTO.Password),
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var defaultRole = await _context.Roles
                .FirstOrDefaultAsync(r => r.Rolename == "User");

            if (defaultRole == null)
            {
                throw new Exception("Default role not found.");
            }

            user.Roles.Add(defaultRole);

            await _context.SaveChangesAsync();

            var roles = user.Roles
                .Select(r => r.Rolename)
                .ToList();

            var token = GnerateJWTTokenAsync(user, roles);

            var refreshToken = GenerateRefreshToken();

            var refreshTokenEntity = new Refreshtoken
            {
                Userid = user.Userid,
                Token = refreshToken,
                Expiresat = DateTime.UtcNow.AddMinutes(
                    Convert.ToDouble(_configuration["Refresh:ExpiryMinutes"])),
            };

            _context.Refreshtokens.Add(refreshTokenEntity);

            await _context.SaveChangesAsync();

            return new LoginResultDTO
            {
                RefreshToken = refreshToken,
                AuthResponse = new AuthResponseDto
                {
                    UserId = user.Userid,
                    Username = user.Username,
                    Token = token,
                    Roles = user.Roles.Select(x => x.Rolename).ToList(),
                }
            };

        }
    }
}
