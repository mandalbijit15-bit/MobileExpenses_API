using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            
            var categories = await _mobileExpensesDbContext.Categories
                .Include(x=>x.Subcategories)
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

    }
}
