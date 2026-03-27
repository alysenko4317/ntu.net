using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace MyNamespace // Change this to your actual namespace
{
    // ------------------------------------------------------------------
    // ITimeService
    // ------------------------------------------------------------------

    public interface ITimeService {
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

    // ------------------------------------------------------------------
    // TimeMessage
    // ------------------------------------------------------------------

    class TimeMessage
    {
        ITimeService timeService;

        public TimeMessage(ITimeService timeService) {
            this.timeService = timeService;
        }

        public string GetTime() => $"Time: {timeService.GetTime()}";
    }

    // ------------------------------------------------------------------
    // Main
    // ------------------------------------------------------------------

    public class Application
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder();

            builder.Services.AddTransient<ITimeService, ShortTimeService>();
            builder.Services.AddTransient<TimeMessage>();

            var app = builder.Build();

            app.Run(async context => {
                var timeMessage = context.RequestServices.GetService<TimeMessage>();
                context.Response.ContentType = "text/html;charset=utf-8";
                await context.Response.WriteAsync($"<h2>{timeMessage?.GetTime()}</h2>");
            });

            app.Run();

        }
    }
}