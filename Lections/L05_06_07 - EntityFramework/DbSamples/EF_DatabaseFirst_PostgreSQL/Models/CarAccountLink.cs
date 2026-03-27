using System;
using System.Collections.Generic;

namespace EFDemo_DBFirstPG.Models;

public partial class CarAccountLink
{
    public int AccountId { get; set; }

    public int CarId { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual Car Car { get; set; } = null!;
}
