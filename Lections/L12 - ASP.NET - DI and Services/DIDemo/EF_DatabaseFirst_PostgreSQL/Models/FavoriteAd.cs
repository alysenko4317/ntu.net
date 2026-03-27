using System;
using System.Collections.Generic;

namespace EFDemo_DBFirstPG.Models;

public partial class FavoriteAd
{
    public int AccountId { get; set; }

    public int AdvertisementId { get; set; }

    public virtual Advertisement Advertisement { get; set; } = null!;
}
