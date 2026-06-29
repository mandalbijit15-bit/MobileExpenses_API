using MobileExpenses_API.DTOs.RequestDTO;

namespace MobileExpenses_API.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardDTO> GetDashboardDetails(int userId);
        Task<bool> SetMonthlyBalance(UpdateBalanceDto updateBalanceDto);
    }
}
