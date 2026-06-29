namespace MobileExpenses_API.DTOs.RequestDTO
{
    public class DashboardDTO
    {
        public int UserId { get; set; }

        public decimal MonthlyBalance { get; set; }

        public decimal RemainingBalance { get; set; }

        public decimal TotalExpenses { get; set; }
    }
}
