using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MobileExpenses_API.DTOs.RequestDTO;
using MobileExpenses_API.Interfaces;
using MobileExpenses_API.Models;


namespace MobileExpenses_API.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MobileExpensesDataController : ControllerBase
    {
        public readonly MobileExpensesDbContext _mobileExpensesDbContext;
        public readonly ICategoryService _categoryService;
        public readonly ITransactionService _transactionService;
        public MobileExpensesDataController(MobileExpensesDbContext mobileExpensesDbContext , ICategoryService categoryService , ITransactionService transactionService)
        {
            _mobileExpensesDbContext = mobileExpensesDbContext;
            _categoryService = categoryService;
            _transactionService = transactionService;

        }

        [HttpGet("GetCategories")]
        public async Task<IActionResult> GetCategories()
        {

            var categories = await _categoryService.GetCategories();

            return Ok(categories);
        }

        [HttpGet("GetTransactionsByUserId")]
        public async Task<IActionResult> GetTransactions(int UserId)
        {
            var transactions = await _transactionService.GetTransactions(UserId);
            return Ok(transactions);

        }
        [HttpPost("AddTransaction")]
        public async Task<IActionResult> AddTransaction(TransactionDTO transaction)
        {
           var savedTransaction = await _transactionService.AddTransaction(transaction);

            return Ok(savedTransaction);
        }

        [HttpDelete("DeleteTransaction/{id}")]
        public async Task<IActionResult> DeleteTransaction(int Id)
        {
            await _transactionService.DeleteTransaction(Id);

            return Ok(Id);
        }

        [HttpPost("UpdateTransaction/{TransactionId}")]
        public async Task<IActionResult> UpdateTransaction(int TransactionId, TransactionDTO transaction)
        {
            await _transactionService.UpdateTransaction(TransactionId, transaction);
            return Ok(transaction);
        }
        [HttpDelete("ClearAllTransactions")]
        public async Task<IActionResult> clearAllTransactions()
        {
            await _transactionService.ClearAllTransactions();
            return Ok();
        }
    }
}
