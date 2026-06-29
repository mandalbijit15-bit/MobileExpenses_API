using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MobileExpenses_API.Interfaces;
using MobileExpenses_API.DTOs.RequestDTO;

namespace MobileExpenses_API.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("GetDashboardDetails/{userId}")]
        public async Task<IActionResult> GetDashboard(int userId)
        {
            var result = await _dashboardService.GetDashboardDetails(userId);

            if (result == null)
            {
                return NotFound("User Not Found");
            }

            return Ok(result);
        }

        [HttpPut("SetBalance")]
        public async Task<IActionResult> SetBalance(
            UpdateBalanceDto updateBalanceDto)
        {
            var result = await _dashboardService.SetMonthlyBalance(updateBalanceDto);

            if (!result)
            {
                return NotFound();
            }

            return Ok(updateBalanceDto.MonthlyBalance);
        }
    }
}
