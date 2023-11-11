var builder = WebApplication.CreateBuilder();
var app = builder.Build();

app.Run(async (context) =>
{
    if (context.Request.Cookies.ContainsKey("name"))
    {
        string? name = context.Request.Cookies["name"];  // отримання
        await context.Response.WriteAsync($"Hello {name}!");
    }
    else
    {
        context.Response.Cookies.Append("name", "Tom");  // встановлення
        await context.Response.WriteAsync("Hello World!");
    }
});

app.Run();

