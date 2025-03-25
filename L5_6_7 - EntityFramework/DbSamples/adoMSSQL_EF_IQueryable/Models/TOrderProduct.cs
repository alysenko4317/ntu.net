using System;
using System.Collections.Generic;

namespace ConsoleApp2.Models;

public partial class TOrderProduct
{
    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public int? Count { get; set; }

    public virtual TOrder Order { get; set; } = null!;

    public virtual TProduct Product { get; set; } = null!;
}
