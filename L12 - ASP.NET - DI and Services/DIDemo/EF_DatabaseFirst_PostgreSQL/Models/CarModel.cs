using System;
using System.Collections.Generic;

namespace EFDemo_DBFirstPG.Models;

public partial class CarModel
{
    public int CarModelId { get; set; }

    public string? Make { get; set; }

    public string? Model { get; set; }

    public int? ModelCarModelId { get; set; }

    public int? CarModelAge { get; set; }

    public virtual Car CarModelNavigation { get; set; } = null!;

    public virtual ICollection<Car> Cars { get; set; } = new List<Car>();

    public virtual ICollection<CarModel> InverseModelCarModel { get; set; } = new List<CarModel>();

    public virtual CarModel? ModelCarModel { get; set; }
}
