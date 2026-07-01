using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MobileExpenses_API.DTOs.RequestDTO;
using MobileExpenses_API.DTOs.ResponseDTO;
using MobileExpenses_API.Interfaces;
using MobileExpenses_API.Models;

namespace MobileExpenses_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register(
            [FromBody] RegisterDTO registerDTO)
        {
            try
            {
                var Loginresult = await _authService.RegisterAsync(registerDTO);

                Response.Cookies.Append(
               "refreshToken",
               Loginresult.RefreshToken,
               new CookieOptions
               {
                   HttpOnly = true,
                   Secure = true,          // true in production (HTTPS)
                   SameSite = SameSiteMode.None, // if frontend and API are on different domains
                   Expires = DateTimeOffset.UtcNow.AddDays(30)
               });

                return Ok(Loginresult.AuthResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    ex.Message
                });
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login(
            [FromBody] LoginDTO loginDTO)
        {
            try
            {
                var Loginresult = await _authService.LoginAsync(loginDTO);

                Response.Cookies.Append(
               "refreshToken",
               Loginresult.RefreshToken,
               new CookieOptions
               {
                   HttpOnly = true,
                   Secure = true,          // true in production (HTTPS)
                   SameSite = SameSiteMode.None, // if frontend and API are on different domains
                   Expires = DateTimeOffset.UtcNow.AddMinutes(10)
               });

                return Ok(Loginresult.AuthResponse);
            }
            catch (Exception ex)
            {
                return Unauthorized(new
                {
                    ex.Message
                });
            }
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(refreshToken))
                return Unauthorized();

            var storedToken = await _authService.Refresh(refreshToken);

            if (storedToken == null)
                return Unauthorized();

            if (storedToken.Isrevoked)
                return Unauthorized();

            if (storedToken.Expiresat <= DateTime.UtcNow)
            {
                await _authService.Logout(refreshToken);
                return Unauthorized();
            }

            var accessToken =
                _authService.GnerateJWTTokenAsync(storedToken.User, storedToken.User.Roles.Select(r => r.Rolename).ToList());

            return Ok(new
            {
                AccessToken = accessToken
            });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if (!string.IsNullOrEmpty(refreshToken))
            {
                var response = await _authService.Logout(refreshToken);
              
                Response.Cookies.Delete("refreshToken");

                return Ok(response);
            }

            return NotFound("Refresh Token not found in request cookie");

        }
    }
}
