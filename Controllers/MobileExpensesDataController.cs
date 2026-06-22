using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MobileExpenses_API.DTOs.RequestDTO;
using MobileExpenses_API.Models;


namespace MobileExpenses_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MobileExpensesDataController : ControllerBase
    {
        public readonly MobileExpensesDbContext _mobileExpensesDbContext;
        public MobileExpensesDataController(MobileExpensesDbContext mobileExpensesDbContext)
        {
            _mobileExpensesDbContext = mobileExpensesDbContext;

        }

        [HttpGet("GetCategories")]
        public async Task<IActionResult> GetCategories()
        {

            var categories = await _mobileExpensesDbContext.Categories
                .Include(x => x.Subcategories)
                .Select(x => new
                {
                    x.Categoryid,
                    x.Categoryname,
                    SubCategories = x.Subcategories.Select(s => new
                    {
                        s.Subcategoryid,
                        s.Subcategoryname,
                    }).ToList()
                }).ToListAsync();


            return Ok(categories);
        }

        [HttpGet("GetTransactions")]
        public async Task<IActionResult> GetTransactions()
        {
            var transactions = await _mobileExpensesDbContext.Transactions
                .Include(x => x.Category)
                .Include(x => x.Subcategory)
                .Select(x => new
                {
                    x.Transactionid,
                    Category = new
                    {
                        x.Category.Categoryid,
                        x.Category.Categoryname
                    },
                    SubCategory = new
                    {
                        x.Subcategory.Subcategoryid,
                        x.Subcategory.Subcategoryname
                    },
                    itemName = x.Itemname,
                    x.Expenseamount
                }).ToListAsync();
            return Ok(transactions);

        }
        [HttpPost("AddTransaction")]
        public async Task<IActionResult> AddTransaction(TransactionRequestDTO transaction)
        {
            _mobileExpensesDbContext.Transactions.Add(new Transaction
            {
                Categoryid = transaction.CategoryId,
                Subcategoryid = transaction.SubCategoryId,
                Itemname = transaction.ItemName,
                Expenseamount = transaction.Expenseamount,
            });
            await _mobileExpensesDbContext.SaveChangesAsync();
            return Ok(transaction);
        }

        [HttpDelete("DeleteTransaction/{id}")]
        public async Task<IActionResult> DeleteTransaction(int Id)
        {
            var transaction = await _mobileExpensesDbContext.Transactions.FindAsync(Id);
            if (transaction == null)
            {
                return NotFound();
            }
            _mobileExpensesDbContext.Transactions.Remove(transaction);
            await _mobileExpensesDbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("UpdateTransaction/{TransactionId}")]
        public async Task<IActionResult> UpdateTransaction(int TransactionId, TransactionRequestDTO transaction)
        {
            var existingTransaction = await _mobileExpensesDbContext.Transactions.FindAsync(TransactionId);
            if (existingTransaction == null)
            {
                return NotFound();
            }
            existingTransaction.Categoryid = transaction.CategoryId;
            existingTransaction.Subcategoryid = transaction.SubCategoryId;
            existingTransaction.Itemname = transaction.ItemName;
            existingTransaction.Expenseamount = transaction.Expenseamount;
            await _mobileExpensesDbContext.SaveChangesAsync();
            return Ok(existingTransaction);
        }
        [HttpDelete("ClearAllTransactions")]
        public async Task<IActionResult> clearAllTransactions()
        {
            var transactions = await _mobileExpensesDbContext.Transactions.ToListAsync();
            _mobileExpensesDbContext.Transactions.RemoveRange(transactions);
            await _mobileExpensesDbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
