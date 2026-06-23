using MobileExpenses_API.DTOs.RequestDTO;

namespace MobileExpenses_API.Interfaces
{
    public interface ITransactionService
    {
        Task<List<TransactionDTO>> GetTransactions();
        Task<TransactionDTO?> AddTransaction(TransactionDTO transaction);

        Task<int?> DeleteTransaction(int transactionId);

        Task<TransactionDTO?> UpdateTransaction(int transactionId, TransactionDTO transaction);

        Task ClearAllTransactions();
    }
}
