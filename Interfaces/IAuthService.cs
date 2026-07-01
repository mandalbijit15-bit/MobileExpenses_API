using Microsoft.AspNetCore.Mvc;
using MobileExpenses_API.DTOs.RequestDTO;
using MobileExpenses_API.DTOs.ResponseDTO;
using MobileExpenses_API.Models;

namespace MobileExpenses_API.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResultDTO> LoginAsync(LoginDTO loginDTO);
        Task<LoginResultDTO> RegisterAsync(RegisterDTO registerDTO);
        string GnerateJWTTokenAsync(User user, List<string> roles);
        Task<Refreshtoken> Refresh(string refreshToken);
        Task<string> Logout(string refreshToken);
        string GenerateRefreshToken();
    }
}
