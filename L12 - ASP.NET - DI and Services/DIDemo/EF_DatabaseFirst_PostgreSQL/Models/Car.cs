using System;
using System.Collections.Generic;

namespace EFDemo_DBFirstPG.Models;

public partial class Car
{
    public int CarId { get; set; }

    public int NumberOfOwners { get; set; }

    public string RegistrationNumber { get; set; } = null!;

    public int ManufactureYear { get; set; }

    public int NumberOfDoors { get; set; }

    public int CarModelId { get; set; }

    public int? Mileage { get; set; }

    public int? Manufactureyear1 { get; set; }

    public int? Mileage1 { get; set; }

    public string? Registrationnumber1 { get; set; }

    public int? AccountId { get; set; }

    public virtual Account? Account { get; set; }

    public virtual ICollection<Advertisement> Advertisements { get; set; } = new List<Advertisement>();

    public virtual CarModel CarModel { get; set; } = null!;

    public virtual CarModel? CarModelNavigation { get; set; }
}
