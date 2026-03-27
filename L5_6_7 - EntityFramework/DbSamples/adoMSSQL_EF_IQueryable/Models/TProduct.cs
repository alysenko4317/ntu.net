using System;
using System.Collections.Generic;

namespace ConsoleApp2.Models;

public partial class TProduct
{
    public int ProductId { get; set; }

    public string? ProductName { get; set; }

    public decimal? ProductPrice { get; set; }

    public virtual ICollection<TOrderProduct> TOrderProducts { get; set; } = new List<TOrderProduct>();
}
