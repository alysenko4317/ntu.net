
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
            // Сутностями вважаються базові типи колекцій DbSet у контексті.
            // Сутностями також є всі типи, куди посилаються сутності, і це правило рекурсивно.
            //
            // цій властивості присвоюється початкове значення - результат методу Set<User> у вигляді об'єкта DbSet<User>.
            // Насправді в функціональному плані в цій ініціалізації мало сенсу, вона ніяк не спричинить роботу властивості,
            // оскільки всі властивості контексту, які представляють об'єкт DbSet, ініціалізуються автоматично при створенні
            // об'єкта контексту. Однак оскільки тип DbSet - тип посилань, явна ініціалізація властивостей типів посилань
            // дозволяє нам обійти попередження статичного аналізу для даних типів посилань, які не ініціалізовані і при цьому
            // не допускають значення null.
            //
            // Як альтернативу можна використовувати вираз null!
            // In C#, the = null! assignment is a way to inform the compiler to suppress nullability warnings for a specific
            // property or variable. It's a feature that comes with the nullable reference types introduced in C# 8.0.

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
