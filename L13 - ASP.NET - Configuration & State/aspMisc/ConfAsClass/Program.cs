
namespace App
{
    public class Person
    {
        public string Name { get; set; } = "";
        public int Age { get; set; } = 0;
    }

    public class Application
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            builder.Configuration.AddJsonFile("person.json");

            // option 1
            //var tom = new Person();
            //app.Configuration.Bind(tom);    // зв'язуємо конфігурацію з об'єктом tom

            // option 2
            Person tom = app.Configuration.Get<Person>();

            // option 3 - DI usage
            app.Map("/", (IConfiguration appConfig) =>
            {
                var tom = appConfig.Get<Person>();  // зв'язуємо конфігурацію з об'єктом tom
                return $"{tom.Name} - {tom.Age}";
            });

            app.Run(async (context) =>
                await context.Response.WriteAsync($"{tom.Name} - {tom.Age}"));

            app.Run();
        }
    }
}

