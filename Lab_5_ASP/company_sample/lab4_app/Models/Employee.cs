using System.ComponentModel.DataAnnotations;

namespace laba_3_1.Models
{
    public class Employee
    {

            public int EmployeeId { get; set; }

            public int? DepartmentId { get; set; }

            public virtual Department Department { get; set; }  // many to one

            [Required(ErrorMessage = "Пожалуйста, введите свое имя")]
            [RegularExpression("[a-zA-Z]", ErrorMessage = "Вы ввели некорректное имя")]
            public string Name { get; set; }
        
    }
}
