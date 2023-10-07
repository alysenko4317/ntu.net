using System;
using System.Collections.Generic;

namespace EFDemo_DBFirstPG.Models;

public partial class AdvertisementPicture
{
    public int AdvertisementPictureId { get; set; }

    public int? AdvertisementId { get; set; }

    public string? PictureLocation { get; set; }

    public virtual Advertisement? Advertisement { get; set; }
}
