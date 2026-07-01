using MobileExpenses_API.DTOs.ResponseDTO;

namespace MobileExpenses_API.DTOs.RequestDTO
{
    public class LoginResultDTO
    {
        public AuthResponseDto AuthResponse { get; set; } = null!;
        public string RefreshToken { get; set; } = string.Empty;
    }
}
