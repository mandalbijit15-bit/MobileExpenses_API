using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MobileExpenses_API.DTOs.RequestDTO;
using MobileExpenses_API.DTOs.ResponseDTO;
using MobileExpenses_API.Interfaces;

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
                var result = await _authService.RegisterAsync(registerDTO);

                return Ok(result);
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
                var result = await _authService.LoginAsync(loginDTO);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return Unauthorized(new
                {
                    ex.Message
                });
            }
        }
    }
}
