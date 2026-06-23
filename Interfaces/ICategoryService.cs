using MobileExpenses_API.DTOs.RequestDTO;

namespace MobileExpenses_API.Interfaces
{
    public interface ICategoryService
    {
        public Task<List<CategoryDTO>> GetCategories();
    }
}
