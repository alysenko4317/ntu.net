using System;
using System.Collections.Generic;

namespace EFDemo_DBFirstPG.Models;

public partial class AdvertisementRating
{
    public int AdvertisementRatingId { get; set; }

    public int AdvertisementId { get; set; }

    public int AccountId { get; set; }

    public DateOnly AdvertisementRatingDate { get; set; }

    public int Rank { get; set; }

    public string Review { get; set; } = null!;

    public virtual Advertisement Advertisement { get; set; } = null!;
}
