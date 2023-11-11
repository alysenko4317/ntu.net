
public class Application
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder();
        var app = builder.Build();

        app.Use(async (context, next) =>
        {
            context.Items["text"] = "Hello from HttpContext.Items";
            await next.Invoke();
        });

        app.Run(async (context) =>
            await context.Response.WriteAsync($"Text: {context.Items["text"]}"));

        app.Run();
    }
}






