using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace app { 

public class ApplicationContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Company> Companies { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
                .UseSqlite("Data Source=helloapp.db")
                .LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);
        }
}

public class Company
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public List<User> Users { get; set; } = new();
}
public class User
{
    public int Id { get; set; }
    public string? Name { get; set; }

    public int? CompanyId { get; set; }
    public Company? Company { get; set; }
}

    class Application
    {
        public static void Main()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                // перевизначимо базу даних
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                // додаємо початкові дані
                Company microsoft = new Company { Name = "Microsoft" };
                Company google = new Company { Name = "Google" };
                db.Companies.AddRange(microsoft, google);


                User tom = new User { Name = "Tom", Company = microsoft };
                User bob = new User { Name = "Bob", Company = google };
                User alice = new User { Name = "Alice", Company = microsoft };
                User kate = new User { Name = "Kate", Company = google };
                db.Users.AddRange(tom, bob, alice, kate);

                db.SaveChanges();
            }
            Console.WriteLine("----------------------------------------------------");
            using (ApplicationContext db = new ApplicationContext())
            {
                Company? company = db.Companies.FirstOrDefault();
                if (company != null)
                {
                    // Вираз db.Users.Where(p=>p.CompanyId==company.Id).Load() завантажує користувачів у контекст.
                    // Підвираз Where(p=>p.CompanyId==company.Id) означає, що завантажуються лише ті користувачі,
                    // у яких властивість CompanyId відповідає властивості Id раніше отриманої компанії
                    db.Users.Where(u => u.CompanyId == company.Id).Load();

                    Console.WriteLine($"Company: {company.Name}");
                    foreach (var u in company.Users)
                        Console.WriteLine($"User: {u.Name}");
                }
            }
        }
    }
}