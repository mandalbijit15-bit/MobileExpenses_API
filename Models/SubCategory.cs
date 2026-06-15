using System;
using System.Collections;
using System.Collections.Generic;

namespace MobileExpenses_API.Models;

public partial class Subcategory
{
    public int Subcategoryid { get; set; }

    public int Categoryid { get; set; }

    public string? Subcategoryname { get; set; }

    public BitArray Isactive { get; set; } = null!;

    public virtual Category Category { get; set; } = null!;
}
