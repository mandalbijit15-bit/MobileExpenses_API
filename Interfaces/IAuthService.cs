using MobileExpenses_API.DTOs.RequestDTO;
using MobileExpenses_API.DTOs.ResponseDTO;
using MobileExpenses_API.Models;

namespace MobileExpenses_API.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(LoginDTO loginDTO);
        Task<AuthResponseDto> RegisterAsync(RegisterDTO registerDTO);
        string GnerateJWTTokenAsync(User user, List<string> roles);
    }
}
