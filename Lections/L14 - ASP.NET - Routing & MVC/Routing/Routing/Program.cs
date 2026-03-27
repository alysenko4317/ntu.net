
namespace App
{
    class Application
    {

        static void Main()
        {
            var builder = WebApplication.CreateBuilder();
            var app = builder.Build();

            app.Map("/users/{id}", (int id) => $"User Id: {id}");
            app.Map("/", () => "Index Page");

            app.Run();
        }
    }
}