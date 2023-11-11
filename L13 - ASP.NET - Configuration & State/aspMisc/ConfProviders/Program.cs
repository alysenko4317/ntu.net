
using Microsoft.Extensions.Configuration;

namespace App { 

public class Application
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();

        app.MapGet("/", (IConfiguration config) => {
            var html = "<html><body><h1>Configuration Sources</h1><table border=\"1\"><tr><th>Source</th></tr>";

            var configurationRoot = config as IConfigurationRoot;
            foreach (var src in configurationRoot.Providers.ToList()) {
                html += $"<tr><td>{src.ToString()}</td></tr>";
            }

            html += "</table></body></html>";
            return Results.Content(html, "text/html");
        });


        var cfg = app.Configuration;
        var mgr = builder.Configuration;

        app.MapGet("/a", () => {
            var html = "<html><body><h1>Configuration Sources</h1><table border=\"1\"><tr><th>Source</th></tr>";

           // var configurationRoot = config as IConfigurationRoot;
            foreach (var src in mgr.Sources)
            {
                html += $"<tr><td>{src.ToString()}</td></tr>";
            }

            html += "</table></body></html>";
            return Results.Content(html, "text/html");
        });

        app.Run();
    }
}

}
