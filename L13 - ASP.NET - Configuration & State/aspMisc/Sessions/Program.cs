var builder = WebApplication.CreateBuilder();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

var app = builder.Build();

app.UseSession();   // додаємо middleware для роботи з сесіями

app.Run(async (context) =>
{
    if (context.Session.Keys.Contains("name"))
        await context.Response.WriteAsync($"Hello {context.Session.GetString("name")}!");
    else
    {
        context.Session.SetString("name", "Tom");
        await context.Response.WriteAsync("Hello World!");
    }
});

app.Run();
