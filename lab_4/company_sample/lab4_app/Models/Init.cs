namespace laba_3_1.Models
{
    public class Init
    {
        public static void InitData()
        {
            Console.WriteLine("Initialize the database with sample data...");

            CompanyModelContainer db = new CompanyModelContainer();

            Department d1 = new Department
            {
                DepartmentName = "Deaprtment 1",
            };

            Department d2 = new Department
            {
                DepartmentName = "Deaprtment 2",
            };

            Department d3 = new Department
            {
                DepartmentName = "Deaprtment 3",
            };

            db.Departments.Add(d1);
            db.Departments.Add(d2);
            db.Departments.Add(d3);

            Employee e1 = new Employee
            {
                Name = "Spencer Phil"
            };

            Employee e2 = new Employee
            {
                Name = "Alekseev Aleksey"
            };

            Employee e3 = new Employee
            {
                Name = "Tmp Tmp"
            };

            Employee e4 = new Employee
            {
                Name = "Bondarenko Alexandr"
            };

            Employee e5 = new Employee
            {
                Name = "Shevchenko Taras"
            };

            Employee e6 = new Employee
            {
                Name = "Antonov Anton"
            };

            db.Workers.Add(e1);
            db.Workers.Add(e2);
            db.Workers.Add(e3);
            db.Workers.Add(e4);
            db.Workers.Add(e5);
            db.Workers.Add(e6);

            d1.Workers.Add(e1);
            d2.Workers.Add(e2);
            d1.Workers.Add(e3);
            d3.Workers.Add(e4);
            d3.Workers.Add(e5);
            d1.Workers.Add(e6);
            // Сохранение данных в БД
            db.SaveChanges();
        }
    }
}
