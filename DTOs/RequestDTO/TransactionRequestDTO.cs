namespace MobileExpenses_API.DTOs.RequestDTO
{
    public class TransactionRequestDTO
    {
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public string ItemName { get; set; } = null!;
        public decimal Expenseamount { get; set; }
    }
}
