using System.Collections;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace app { 
public class User
{
    public int Id { get; set; }
    public string? Name { get; set; }
}

public class ApplicationContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public ApplicationContext()
    {
        Database.EnsureDeleted();
        Database.EnsureCreated();
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseSqlite("Data Source=helloapp.db")
            .LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // добавляем один объект
        modelBuilder.Entity<User>().HasData(new User { Id = 1, Name = "Tom" });
    }
}

    class Application
    {
        public static void Main()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                // При виконанні запиту IEnumerable завантажує всі дані, і якщо нам потрібно здійснити їх фільтрацію,
                // то фільтрація відбувається на стороні клієнта.
                //IEnumerable<User> userIEnum = db.Users;

                // Об’єкт IQueryable забезпечує віддалений доступ до бази даних та дозволяє переміщуватися по даних
                // як у прямому порядку від початку до кінця, так і у зворотному порядку. Під час створення запиту,
                // об’єктом, що повертається, є IQueryable, що дозволяє оптимізувати запит; безпосередній вибір даних
                // відбувається під час виклику .ToList(), а фільтрація в цьому випадку здійснюється на стороні СУБД.
                //
                // IQueryable дозволяє динамічно створювати складні запити. Наприклад, можна послідовно наслаювати
                // умови для фільтрації залежно від потреб:
                //    IQueryable<User> userIQuer = db.Users;
                //    userIQuer = userIQuer.Where(p => p.Id < 10);
                //    userIQuer = userIQuer.Where(p => p.Name == "Tom");
                //    var users = userIQuer.ToList();
                //
                IQueryable<User> userIQuer = db.Users;

                var users = userIQuer.Where(p => p.Id < 10)
                                              .ToList();

              //  var users = (userIQuer as IEnumerable<User>).Where(p => p.Id < 10)
              //                                                       .ToList();

                foreach (var user in users)
                    Console.WriteLine(user.Name);
            }
        }
    }
}