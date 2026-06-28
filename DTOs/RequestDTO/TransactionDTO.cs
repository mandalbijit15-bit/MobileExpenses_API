namespace MobileExpenses_API.DTOs.RequestDTO
{
    public class TransactionDTO
    {
        public int? Transactionid { get; set; }
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public int SubCategoryId { get; set; }
        public string? SubCategoryName { get; set; }
        public string ItemName { get; set; } = null!;
        public decimal Expenseamount { get; set; }
        public int? Userid { get; set; }
    }
}
