namespace App
{
    class Application
    {
        static void Main()
        {
            var builder = WebApplication.CreateBuilder();
            var app = builder.Build();
            /*
            app.Run(async (context) => {
                // виводимо інформацію у консоль
                app.Logger.LogInformation($" *** Processing request {context.Request.Path}");
               */

            app.Map("/hello", (ILogger<Application> logger) => {
                logger.LogInformation($" *** Path: /hello  Time: {DateTime.Now.ToLongTimeString()}");
                return "Hello World";
            });

            app.Run();
        }
    }
}
