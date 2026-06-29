namespace MobileExpenses_API.DTOs.RequestDTO
{
    public class UpdateBalanceDto
    {
        public int UserId { get; set; }
        public decimal MonthlyBalance { get; set; }
    }
}
