namespace MyNamespace // Change this to your actual namespace
{
    // ------------------------------------------------------------------
    // IHelloService
    // ------------------------------------------------------------------

    interface IHelloService {
       string Message { get; }
    }

    class UkHelloService : IHelloService {
       public string Message => "Привіт!";
    }

    class EnHelloService : IHelloService {
       public string Message => "Hello!";
    }

    // ------------------------------------------------------------------
    // HelloMiddleware
    // ------------------------------------------------------------------

    class HelloMiddleware
    {
        readonly IEnumerable<IHelloService> helloServices;

        public HelloMiddleware(RequestDelegate _,
                               IEnumerable<IHelloService> helloServices) {
            this.helloServices = helloServices;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Response.ContentType = "text/html; charset=utf-8";
            string responseText = "";
            foreach (var service in helloServices)
                 responseText += $"<h3>{service.Message}</h3>";
            await context.Response.WriteAsync(responseText);
        }
    }

    // ------------------------------------------------------------------
    // Main
    // ------------------------------------------------------------------

    public class Application
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder();
            builder.Services.AddTransient<IHelloService, UkHelloService>();
            builder.Services.AddTransient<IHelloService, EnHelloService>();

            var app = builder.Build();

            app.UseMiddleware<HelloMiddleware>();

            app.Run();
        }
    }
}