namespace App
{
    class Application
    {
        static void Main()
        {
            var builder = WebApplication.CreateBuilder();
            var app = builder.Build();

            app.Use(async (context, next) =>
            {
                Console.WriteLine("First middleware starts");
                await next.Invoke();
                Console.WriteLine("First middleware ends");
            });

            app.Map("/", () =>
            {
                Console.WriteLine("Index endpoint starts and ends");
                return "Index Page";
            });

            app.Use(async (context, next) =>
            {
                Console.WriteLine("Second middleware starts");
                await next.Invoke();
                Console.WriteLine("Second middleware ends");
            });

            app.Map("/about", () =>
            {
                Console.WriteLine("About endpoint starts and ends");
                return "About Page";
            });

            app.Run();
        }
    }
}
