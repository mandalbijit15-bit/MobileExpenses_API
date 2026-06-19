using System;
using System.Collections.Generic;

namespace MobileExpenses_API.Models;

public partial class Transaction
{
    public int Transactionid { get; set; }

    public int Categoryid { get; set; }

    public int Subcategoryid { get; set; }

    public string Itemname { get; set; } = null!;

    public decimal Expenseamount { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual Subcategory Subcategory { get; set; } = null!;
}
