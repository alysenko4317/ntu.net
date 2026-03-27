using System;
using System.Collections.Generic;

namespace ConsoleApp2.Models;

public partial class TOrder
{
    public int OrderId { get; set; }

    public int? CustomerId { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual TCustomer? Customer { get; set; }

    public virtual ICollection<TOrderProduct> TOrderProducts { get; set; } = new List<TOrderProduct>();
}
