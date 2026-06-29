using Microsoft.EntityFrameworkCore;
using MobileExpenses_API.DTOs.RequestDTO;
using MobileExpenses_API.Interfaces;
using MobileExpenses_API.Models;

namespace MobileExpenses_API.Services
{
        public class TransactionService : ITransactionService
        {
            private readonly MobileExpensesDbContext _mobileExpensesDbContext;

            public TransactionService(MobileExpensesDbContext mobileExpensesDbContext)
            {
                _mobileExpensesDbContext = mobileExpensesDbContext;
            }

            public async Task<List<TransactionDTO>> GetTransactions(int UserId)
            {
                return await _mobileExpensesDbContext.Transactions
                .Include(x => x.Category)
                .Include(x => x.Subcategory)
                .Where(x=>x.Userid == UserId)
                .Select(x => new TransactionDTO
                {
                    Transactionid = x.Transactionid,
                    CategoryId = x.Categoryid,
                    CategoryName = x.Category.Categoryname,
                    SubCategoryId = x.Subcategory.Subcategoryid,
                    SubCategoryName = x.Subcategory.Subcategoryname,
                    Expenseamount = x.Expenseamount,
                    ItemName = x.Itemname,
                    Userid = UserId,

                }).ToListAsync();
            }

            public async Task<int?> DeleteTransaction(int transactionId)
            {
                var transaction = await _mobileExpensesDbContext.Transactions
                    .FindAsync(transactionId);

                if (transaction == null)
                {
                    return null;
                }

                _mobileExpensesDbContext.Transactions.Remove(transaction);
                await _mobileExpensesDbContext.SaveChangesAsync();

                return transactionId;
            }

            public async Task<TransactionDTO?> UpdateTransaction(
                int transactionId,
                TransactionDTO transaction)
            {
                var existingTransaction = await _mobileExpensesDbContext.Transactions
                    .FindAsync(transactionId);

                if (existingTransaction == null)
                {
                    return null;
                }

                existingTransaction.Categoryid = transaction.CategoryId;
                existingTransaction.Subcategoryid = transaction.SubCategoryId;
                existingTransaction.Itemname = transaction.ItemName;
                existingTransaction.Expenseamount = transaction.Expenseamount;

                await _mobileExpensesDbContext.SaveChangesAsync();

                return transaction;
            }

            public async Task clearAllTransactionsByUserid(int userid)
            {
                var transactions = await _mobileExpensesDbContext.Transactions
                .Where(t=>t.Userid == userid)
                    .ToListAsync();

                _mobileExpensesDbContext.Transactions.RemoveRange(transactions);

                await _mobileExpensesDbContext.SaveChangesAsync();
            }

            public async Task<TransactionDTO?> AddTransaction(TransactionDTO transaction)
            {
                var transactionEntity = new Transaction
                {
                    Categoryid = transaction.CategoryId,
                    Subcategoryid = transaction.SubCategoryId,
                    Itemname = transaction.ItemName,
                    Expenseamount = transaction.Expenseamount,
                    Userid = transaction.Userid,
                };

                _mobileExpensesDbContext.Transactions.Add(transactionEntity);

                await _mobileExpensesDbContext.SaveChangesAsync();

                // transactionEntity.Transactionid is now populated

                return await _mobileExpensesDbContext.Transactions
                    .Where(x => x.Transactionid == transactionEntity.Transactionid)
                    .Select(x => new TransactionDTO
                    {
                        Transactionid = x.Transactionid,
                        CategoryId = x.Categoryid,
                        CategoryName = x.Category.Categoryname,
                        SubCategoryId = x.Subcategory.Subcategoryid,
                        SubCategoryName = x.Subcategory.Subcategoryname,
                        Expenseamount = x.Expenseamount,
                        ItemName = x.Itemname,
                        Userid = x.Userid
                    })
                    .FirstOrDefaultAsync();
            }
        }
}
