
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using static sqlite_app.Program;

// 0. install Microsoft.EntityFrameworkCore
//            Microsoft.EntityFrameworkCore.Sqlite

// 1. Определить модели сущностей.
// 2. Определить модель контекста.
// 3. Прописать строку соединения

namespace sqlite_app
{
    class Program
    {
        public class User
        {
            public int Id { get; set; }  // or UserId
            public string? Name { get; set; }
            public int Age { get; set; }
           // public int someName { get; set; }
        }

        public class MyDbContext : DbContext
        {
            public DbSet<User> Users { get; set; } = null!;

            public MyDbContext(DbContextOptions<MyDbContext> options)
                : base(options)
            {
                Database.EnsureDeleted();
                Database.EnsureCreated();
            }
        }

        public class ConfigurationLoader
        {
            private readonly string _configFileName;

            public ConfigurationLoader(string configFileName = "appsettings.json") {
                _configFileName = configFileName;
            }

            public string LoadConnectionString(string connectionName = "DefaultConnection")
            {
                if (! File.Exists(Path.Combine(Directory.GetCurrentDirectory(), _configFileName))) {
                    throw new FileNotFoundException($"Error: {_configFileName} not found.");
                }

                // Для створення конфігурації використовується клас ConfigurationBuilder.
                // Метод AddJsonFile() додає всі параметри з конфігураційного файлу.
                // За допомогою методу Build() створюється об'єкт конфігурації, з якого ми можемо
                // отримати рядок підключення:

                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile(_configFileName);

                var config = builder.Build();

                var connectionString = config.GetConnectionString(connectionName);
                if (string.IsNullOrEmpty(connectionString)) {
                    throw new InvalidOperationException(
                        $"Error: No connection string named '{connectionName}' found in {_configFileName}.");
                }

                return connectionString;
            }
        }

        static void Main(string[] args)
        {
            try
            {
                var configurationLoader = new ConfigurationLoader();
                string connectionString = configurationLoader.LoadConnectionString();

                //var optionsBuilder = new DbContextOptionsBuilder<MyDbContext>();
                //var options = optionsBuilder.UseSqlite(connectionString).Options;

                var optionsBuilder = new DbContextOptionsBuilder<MyDbContext>()
                        .UseSqlite(connectionString)
                        .LogTo(Console.WriteLine, LogLevel.Information);  // Logging EF operations to Console
                
                // The LogTo method accepts multiple parameters which can be used to customize the logging behavior,
                // such as filtering which logs to capture based on their log level. In the example above, I've chosen
                // to capture logs at the Information level, but you can adjust this to your needs.

                var options = optionsBuilder.Options;

                using (MyDbContext db = new MyDbContext(options))
                {
                    // create objects
                    User tom = new User { Name = "Tom", Age = 33 };
                    User alice = new User { Name = "Alice", Age = 26 };

                    // adding them to DB
                    db.Users.Add(tom);
                    db.Users.Add(alice);
                    db.SaveChanges();
                    Console.WriteLine("Objects are saved succesully");

                    // retriving objects from DB and print
                    var users = db.Users.ToList();
                    Console.WriteLine("List of objects:");
                    foreach (User u in users)
                    {
                        Console.WriteLine($"{u.Id}.{u.Name} - {u.Age}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}