using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace MyNamespace // Change this to your actual namespace
{
    public interface ITimeService
    {
        string GetTime();
    }

    public class ShortTimeService : ITimeService {
        public string GetTime() {
            return DateTime.Now.ToShortTimeString();
        }
    }

    public class LongTimeService : ITimeService {
        public string GetTime() {
            return DateTime.Now.ToLongTimeString();
        }
    }

    // optional: extension method definition
    public static class ServiceProviderExtensions {
        public static void AddTimeService(this IServiceCollection services) {
            services.AddTransient<ITimeService, ShortTimeService>();
        }
    }


    public class Application
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Registering the ShortTimeService with DI container
            // Додавання сервісу в колекцію сервісів здійснюється до створення об’єкта WebApplication методом Build().
            // Завдяки даному виклику система на місце об’єктів інтерфейсу ITimeService буде передавати екземпляри класу ShortTimeService. 

            builder.Services.AddTransient<ITimeService, ShortTimeService>();
            //builder.Services.AddTimeService();

            var app = builder.Build();

            app.MapGet("/", async context =>
            {
                                         //   .GetService (can return null)
                var timeService = app.Services.GetRequiredService<ITimeService>();
                await context.Response.WriteAsync($"Time: {timeService.GetTime()}");
            });

            app.Run();
        }
    }
}
