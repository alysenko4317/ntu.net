namespace laba_3_1.Models
{
    public class Department
    {
        public Department()
        {
            Workers = new List<Employee>();
        }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public virtual List<Employee> Workers { get; set; }
    }
}
