using System;
using System.Collections.Generic;

namespace ConsoleApp2.Models;

public partial class TCustomer
{
    public int CustomerId { get; set; }

    public string? CustomerName { get; set; }

    public virtual ICollection<TOrder> TOrders { get; set; } = new List<TOrder>();
}
