
namespace HelloWorld_5
{
    public class Program
    {
        // ---------------------------------------------------------
        // SAMPLE 1: Map() demonstration
        // ---------------------------------------------------------

        public static void Main_1(string[] args)
        {
            var app = WebApplication.CreateBuilder(args).Build();

            app.Map("/index", appBuilder => {
                appBuilder.Run(async context => await context.Response.WriteAsync("Index Page"));
            });

            app.Map("/about", appBuilder => {
                appBuilder.Run(async context => await context.Response.WriteAsync("About Page"));
            });

            app.Run(async context => await context.Response.WriteAsync("Page Not Found"));

            app.Run();
        }

        // ---------------------------------------------------------
        // SAMPLE 2: Map() demonstration
        // ---------------------------------------------------------

        static void Index(IApplicationBuilder appBuilder)
        {
            appBuilder.Run(async context => await context.Response.WriteAsync("Index Page"));
        }

        static void About(IApplicationBuilder appBuilder)
        {
            appBuilder.Run(async context => await context.Response.WriteAsync("About Page"));
        }

        public static void Main(string[] args)
        {
            var app = WebApplication.CreateBuilder(args).Build();

            app.Map("/home", appBuilder => {
                appBuilder.Map("/index", Index); // middleware для "/home/index"
                appBuilder.Map("/about", About); // middleware для "/home/about"

                // middleware для "/home"
                appBuilder.Run(async context => await context.Response.WriteAsync("Home Page"));
            });

            app.Run(async context => await context.Response.WriteAsync("Page Not Found"));

            app.Run();
        }
    }
}