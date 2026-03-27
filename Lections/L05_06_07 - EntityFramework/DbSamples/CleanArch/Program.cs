using MyApp.Infrastructure.Persistence;
using MyApp.Framework.Database;
using Microsoft.EntityFrameworkCore;
using MyApp.Core.Interfaces;

namespace MyApp
{
    // Використання архітектрного патерну Шаблонний метод

    public abstract class ApplicationBase
    {
        public void Run()
        {
            var options = ConfigureDatabase();

            using (var dbContext = new DatabaseContext(options))
            {
                dbContext.Database.EnsureCreated();

                AddTestData(dbContext);

                IUserRepository userRepository = new UserRepository(dbContext);
                var consoleUserService = new Presentation.ConsoleService(userRepository);
                consoleUserService.DisplayAllUsers();
            }

            Console.ReadLine(); // Щоб не закривати консоль одразу
        }

        // Метод для налаштування бази даних (переопределяється в підкласах)
        protected abstract DbContextOptions<DatabaseContext> ConfigureDatabase();

        // Метод для додавання тестових даних (можна зробити віртуальним або статичним)
        protected virtual void AddTestData(DatabaseContext dbContext)
        {
            dbContext.Users.Add(new Core.Entities.User { Name = "John Doe", Email = "john@example.com" });
            dbContext.Users.Add(new Core.Entities.User { Name = "Jane Smith", Email = "jane@example.com" });
            dbContext.SaveChanges();
        }
    }

    public class InMemoryApplication : ApplicationBase {
        protected override DbContextOptions<DatabaseContext> ConfigureDatabase()
        {
            return new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
        }
    }

    public class SqliteApplication : ApplicationBase {
        protected override DbContextOptions<DatabaseContext> ConfigureDatabase()
        {
            return new DbContextOptionsBuilder<DatabaseContext>()
                .UseSqlite("Data Source=mydatabase.db")
                .Options;
        }
    }

    public class SqlServerApplication : ApplicationBase {
        protected override DbContextOptions<DatabaseContext> ConfigureDatabase()
        {
            // Замініть connectionString на реальний рядок підключення до вашої MS SQL бази
            string connectionString = "Server=your_server_name;Database=your_database_name;User Id=your_username;Password=your_password;";

            return new DbContextOptionsBuilder<DatabaseContext>()
                .UseSqlServer(connectionString)
                .Options;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            ApplicationBase app;

            app = new SqliteApplication();
            //app = new InMemoryApplication();

            app.Run();
        }
    }
}
