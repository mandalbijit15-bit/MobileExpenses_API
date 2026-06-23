using MobileExpenses_API.DTOs;

namespace MobileExpenses_API.DTOs.RequestDTO
{
    public class CategoryDTO
    {
        public int Categoryid { get; set; }
        public string? Categoryname { get; set; }
        public List<SubcategoryDTO>? Subcategory { get; set; }

    }
}
