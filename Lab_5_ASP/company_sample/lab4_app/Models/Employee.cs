using System.ComponentModel.DataAnnotations;

namespace laba_3_1.Models
{
    public class Employee
    {

            public int EmployeeId { get; set; }

            public int? DepartmentId { get; set; }

            public virtual Department Department { get; set; }  // many to one

            [Required(ErrorMessage = "Будь ласка, введіть своє ім'я")]
            [RegularExpression("[a-zA-Z]", ErrorMessage = "Ви ввели некоректне ім'я")]
            public string Name { get; set; }
        
    }
}
