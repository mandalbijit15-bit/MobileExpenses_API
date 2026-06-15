using System;
using System.Collections.Generic;

namespace MobileExpenses_API.Models;

public partial class SubCategory
{
    public int SubCategoryId { get; set; }

    public int CategoryId { get; set; }

    public string SubCategoryName { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual Category Category { get; set; } = null!;
}
