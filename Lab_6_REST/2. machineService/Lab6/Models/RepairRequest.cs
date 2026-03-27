using System;

namespace Lab6.Models
{
    public class RepairRequest
    {
        public Guid WorkerId { get; set; }
        public string CarName { get; set; }
        public string CarRegistrationNumber { get; set; }
        // можна додати і інші поля, наприклад:
        //   public string CarModel { get; set; }
        //   public string CarColor { get; set; }
        //   public string Comment { get; set; }
    }
}
