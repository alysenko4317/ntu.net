using System;
using System.Collections.Generic;

namespace EFDemo_DBFirstPG.Models;

public partial class SellerAccount
{
    public int SellerAccountId { get; set; }

    public int AccountId { get; set; }

    public double? TotalRank { get; set; }

    public int? NumberOfAdvertisement { get; set; }

    public string StreetName { get; set; } = null!;

    public string StreetNumber { get; set; } = null!;

    public string ZipCode { get; set; } = null!;

    public string City { get; set; } = null!;

    public virtual ICollection<Advertisement> Advertisements { get; set; } = new List<Advertisement>();
}
