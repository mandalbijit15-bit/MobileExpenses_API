using System;
using System.Collections;
using System.Collections.Generic;

namespace MobileExpenses_API.Models;

public partial class Category
{
    public int Categoryid { get; set; }

    public string? Categoryname { get; set; }

    public BitArray Isactive { get; set; } = null!;

    public virtual ICollection<Subcategory> Subcategories { get; set; } = new List<Subcategory>();
}
