using Microsoft.EntityFrameworkCore;
using MobileExpenses_API.DTOs.RequestDTO;
using MobileExpenses_API.Interfaces;
using MobileExpenses_API.Models;

namespace MobileExpenses_API.Services
{
    public class CategoryService : ICategoryService
    {
        public readonly MobileExpensesDbContext _mobileExpensesDbContext;
        public CategoryService(MobileExpensesDbContext mobileExpensesDbContext )
        {
            _mobileExpensesDbContext = mobileExpensesDbContext;
            
        }

        public async Task<List<CategoryDTO>> GetCategories()
        {
            var Categroies = await _mobileExpensesDbContext.Categories
                .Include(x => x.Subcategories)
                .Select(x => new CategoryDTO
                {
                    Categoryid = x.Categoryid,
                    Categoryname = x.Categoryname,
                    Subcategory = x.Subcategories.Select(s => new SubcategoryDTO
                    {
                       Subcategoryid = s.Subcategoryid,
                      Subcategoryname = s.Subcategoryname
                    }).ToList()
                }).ToListAsync();

            return Categroies;
        }
    }
}
