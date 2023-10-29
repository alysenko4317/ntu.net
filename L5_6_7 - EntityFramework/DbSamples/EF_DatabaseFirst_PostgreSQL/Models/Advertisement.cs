using System;
using System.Collections.Generic;

namespace EFDemo_DBFirstPG.Models;

public partial class Advertisement
{
    public int AdvertisementId { get; set; }

    public DateTime AdvertisementDate { get; set; }

    public int CarId { get; set; }

    public int SellerAccountId { get; set; }

    public virtual ICollection<AdvertisementPicture> AdvertisementPictures { get; set; } = new List<AdvertisementPicture>();

    public virtual ICollection<AdvertisementRating> AdvertisementRatings { get; set; } = new List<AdvertisementRating>();

    public virtual Car Car { get; set; } = null!;

    public virtual ICollection<FavoriteAd> FavoriteAds { get; set; } = new List<FavoriteAd>();

    public virtual SellerAccount SellerAccount { get; set; } = null!;
}
