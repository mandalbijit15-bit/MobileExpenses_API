using Microsoft.EntityFrameworkCore;
using MobileExpenses_API.DTOs.RequestDTO;
using MobileExpenses_API.Interfaces;
using MobileExpenses_API.Models;

namespace MobileExpenses_API.Services
{
    public class DashboardService : IDashboardService
    {
        readonly MobileExpensesDbContext _mobileExpensesDbContext;
        public DashboardService(MobileExpensesDbContext mobileExpensesDbContext)
        {
            _mobileExpensesDbContext = mobileExpensesDbContext;
        }

        public async Task<DashboardDTO> GetDashboardDetails(int userId)
        {
            var user = await _mobileExpensesDbContext.Users
            .FirstOrDefaultAsync(x => x.Userid == userId);

            if (user == null)
            {
                return null;
            }

            var totalExpenses = await _mobileExpensesDbContext.Transactions
                .Where(x => x.Userid == userId)
                .SumAsync(x => x.Expenseamount);

            return new DashboardDTO
            {
                UserId = userId,
                MonthlyBalance = user.Monthlybalance,
                TotalExpenses = totalExpenses,
                RemainingBalance = user.Monthlybalance - totalExpenses
            };
        }

        public async Task<bool> SetMonthlyBalance(UpdateBalanceDto updateBalanceDto)
        {
            var user = await _mobileExpensesDbContext.Users
          .FirstOrDefaultAsync(x => x.Userid == updateBalanceDto.UserId);

            if (user == null)
            {
                return false;
            }

            user.Monthlybalance = updateBalanceDto.MonthlyBalance;

            await _mobileExpensesDbContext.SaveChangesAsync();

            return true;
        }
    }
}
