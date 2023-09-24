
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

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
        }

        public class MyDbContext : DbContext
        {
            // Сущностями считаются базовые типы коллекций DbSet в классе контекста.
            // Сущностями также считаются все типы, на которые ссылаются сущности, и это правило рекурсивно.

            //public DbSet<User> Users { get; set; } = null!;  alternative init
            public DbSet<User> Users => Set<User>();

            public MyDbContext() => Database.EnsureCreated();

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
                optionsBuilder.UseSqlite("Data Source=helloapp.db");  // connection string
            }
        }

        static void Main(string[] args)
        {
            using (MyDbContext db = new MyDbContext())
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
                foreach (User u in users) {
                    Console.WriteLine($"{u.Id}.{u.Name} - {u.Age}");
                }
            }
        }
    }
}
