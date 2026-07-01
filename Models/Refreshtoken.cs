using System;
using System.Collections.Generic;

namespace MobileExpenses_API.Models;

public partial class Refreshtoken
{
    public int Id { get; set; }

    public int Userid { get; set; }

    public string Token { get; set; } = null!;

    public DateTime Createdat { get; set; }

    public DateTime Expiresat { get; set; }

    public bool Isrevoked { get; set; }

    public virtual User User { get; set; } = null!;
}
