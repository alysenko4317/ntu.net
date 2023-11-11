
namespace App
{
    public class Person
    {
        public string Name { get; set; } = "";
        public int Age { get; set; }
        public List<string> Languages { get; set; } = new();
        public Company? Company { get; set; }
    }

    public class Company
    {
        public string Title { get; set; } = "";
        public string Country { get; set; } = "";
    }


    public class Application
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder();
            var app = builder.Build();

            builder.Configuration.AddJsonFile("person.json");

            var tom = new Person();
            app.Configuration.Bind(tom);

            app.Run(async (context) =>
            {
                context.Response.ContentType = "text/html; charset=utf-8";
                string name = $"<p>Name: {tom.Name}</p>";
                string age = $"<p>Age: {tom.Age}</p>";
                string company = $"<p>Company: {tom.Company?.Title}</p>";
                string langs = "<p>Languages:</p><ul>";

                foreach (var lang in tom.Languages) {
                    langs += $"<li><p>{lang}</p></li>";
                }
                langs += "</ul>";

                await context.Response.WriteAsync($"{name}{age}{company}{langs}");
            });

            app.Run();
        }
    }
}
