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
                // При выполнении запроса IEnumerable загружает все данные, и если нам надо выполнить их фильтрацию,
                // то сама фильтрация происходит на стороне клиента.
                //IEnumerable<User> userIEnum = db.Users;

                // Объект IQueryable предоставляет удаленный доступ к базе данных и позволяет перемещаться по данным
                // как в прямом порядке от начала до конца, так и в обратном порядке. В процессе создания запроса,
                // возвращаемым объектом которого является IQueryable, происходит оптимизация запроса, непосредственная
                // выборка данных происходит во время вызова .ToList(), фильтрация в этом случае происходит на стороне СУБД.
                //
                // IQueryable позволяет динамически создавать сложные запросы. Например, мы можем последовательно наслаивать
                // в зависимости от условий выражения для фильтрации:
                //    IQueryable<User> userIQuer = db.Users;
                //    userIQuer = userIQuer.Where(p => p.Id < 10);
                //    userIQuer = userIQuer.Where(p => p.Name == "Tom");
                //    var users = userIQuer.ToList();
                //
                IQueryable<User> userIQuer = db.Users;

                var users = userIQuer.Where(p => p.Id < 10).ToList();

                foreach (var user in users)
                    Console.WriteLine(user.Name);
            }
        }
    }
}