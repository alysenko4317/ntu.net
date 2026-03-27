
namespace App
{
    class Application
    {
        public class Person
        {
            public string Id { get; set; } = "";
            public string Name { get; set; } = "";
            public int Age { get; set; }
        }

        // початкові дані
        static List<Person> users = new List<Person> {
           new() { Id = Guid.NewGuid().ToString(), Name = "Tom", Age = 37 },
           new() { Id = Guid.NewGuid().ToString(), Name = "Bob", Age = 41 },
           new() { Id = Guid.NewGuid().ToString(), Name = "Sem", Age = 24 }
        };

        static void Main()
        {
            var builder = WebApplication.CreateBuilder();
            var app = builder.Build();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.MapGet("/api/users", () => users);

            app.MapGet("/api/users/{id}", (string id) =>
            {
                // отримуємо користувача за id
                Person? user = users.FirstOrDefault(u => u.Id == id);
                // якщо не знайдений, відправляємо статусний код і повідомлення про помилку
                if (user == null)
                    return Results.NotFound(new { message = "Користувач не знайдений" });

                // якщо користувач знайдений, відправляємо його
                return Results.Json(user);
            });

            // postman
            // curl -X DELETE "https://localhost:7222/api/users/85052de8-3279-47dd-8eab-e9848ce22bba"
            app.MapDelete("/api/users/{id}", (string id) =>
            {
                // отримуємо користувача за id
                Person? user = users.FirstOrDefault(u => u.Id == id);

                // якщо не знайдений, відправляємо статусний код і повідомлення про помилку
                if (user == null)
                    return Results.NotFound(new { message = "Користувач не знайдений" });

                // якщо користувача знайдено, видаляємо його
                users.Remove(user);
                return Results.Json(user);
            });

            app.MapPost("/api/users", (Person user) =>
            {

                // встановлюємо id для нового користувача
                user.Id = Guid.NewGuid().ToString();

                // додаємо користувача до списку
                users.Add(user);

                return user;
            });

            app.MapPut("/api/users", (Person userData) =>
            {

                // отримуємо користувача за id
                var user = users.FirstOrDefault(u => u.Id == userData.Id);

                // якщо не знайдений, відправляємо статусний код і повідомлення про помилку

                if (user == null) return Results.NotFound(new { message = "Користувач не знайдений" });
                // якщо користувач знайдений, змінюємо його дані і відправляємо назад клієнту

                user.Age = userData.Age;
                user.Name = userData.Name;

                return Results.Json(user);
            });

            app.Run();
        }
    }
}
