using System;
using System.Collections.Generic;

namespace EFDemo_DBFirstPG.Models;

public partial class Account
{
    public int AccountId { get; set; }

    public string? Email { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Password { get; set; }

    public DateOnly? LoginDate { get; set; }

    public int? OwnerAccountId { get; set; }

    public virtual ICollection<Car> Cars { get; set; } = new List<Car>();

    public virtual ICollection<Account> InverseOwnerAccount { get; set; } = new List<Account>();

    public virtual Account? OwnerAccount { get; set; }
}
