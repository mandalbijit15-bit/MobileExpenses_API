namespace MobileExpenses_API.DTOs.ResponseDTO
{
    public class AuthResponseDto
    {
        public int UserId { get; set; }

        public string Username { get; set; } = null!;

        public string Token { get; set; } = null!;

        public List<string> Roles { get; set; } = null!;
    }

}
