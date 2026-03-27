using Microsoft.Extensions.DependencyInjection;
using System.Reflection.Emit;
using System.Reflection.PortableExecutable;

namespace MyNamespace // Change this to your actual namespace
{
    // ------------------------------------------------------------------
    // Services
    // ------------------------------------------------------------------

    interface IGenerator {
        int GenerateValue();
    }

    interface IReader {
        int ReadValue();
    }

    class ValueStorage : IGenerator, IReader
    {
        int value;

        public int GenerateValue() {
            value = new Random().Next();
            return value;
        }

        public int ReadValue() => value;
    }

    // ------------------------------------------------------------------
    // Middleware
    // ------------------------------------------------------------------

    class GeneratorMiddleware
    {
        RequestDelegate next;
        IGenerator generator;

        public GeneratorMiddleware(RequestDelegate next, IGenerator generator)
        {
            this.next = next;
            this.generator = generator;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path == "/generate")
                await context.Response.WriteAsync(
                    $"New Value: {generator.GenerateValue()}");
            else
                await next.Invoke(context);
        }
    }

    class ReaderMiddleware
    {
        IReader reader;

        public ReaderMiddleware(RequestDelegate _, IReader reader) =>
             this.reader = reader;

        public async Task InvokeAsync(HttpContext context) {
            await context.Response.WriteAsync($"Current Value: {reader.ReadValue()}");
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
            
                        builder.Services.AddSingleton<ValueStorage>();

                        builder.Services.AddSingleton<IGenerator>(
                                            serv => serv.GetRequiredService<ValueStorage>());

                        builder.Services.AddSingleton<IReader>(
                                            serv => serv.GetRequiredService<ValueStorage>());
            
          //  VARIANT 2: 
         //   var vs = new ValueStorage();
         //   builder.Services.AddSingleton<IGenerator>(vs);
         //   builder.Services.AddSingleton<IReader>(vs);

          //  builder.Services.AddSingleton<IGenerator, ValueStorage>();
          //  builder.Services.AddSingleton<IReader, ValueStorage>();

            var app = builder.Build();
            app.UseMiddleware<GeneratorMiddleware>();
            app.UseMiddleware<ReaderMiddleware>();
            app.Run();
        }
    }
}