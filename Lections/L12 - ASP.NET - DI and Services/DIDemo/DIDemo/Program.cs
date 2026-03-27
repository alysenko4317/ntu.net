
using System.Text;

namespace DIDemo {

class Program
{
    static void Main()
    {
        var builder = WebApplication.CreateBuilder();
        var services = builder.Services;
        var app = builder.Build();

        app.Run(async context =>
        {
            var sb = new StringBuilder();
            sb.Append("<h1>”с≥ серв≥си</h1><table>");
            sb.Append("<tr><th>“ип</th><th>Lifetime</th><th>–еал≥зац≥€</th></tr>");
            foreach (var svc in services)
            {
                sb.Append("<tr>");
                sb.Append($"<td>{svc.ServiceType.FullName}</td>");
                sb.Append($"<td>{svc.Lifetime}</td>");
                sb.Append($"<td>{svc.ImplementationType?.FullName}</td>");
                sb.Append("</tr>");
            }
            sb.Append("</table>");
            context.Response.ContentType = "text/html;charset=utf-8";
            await context.Response.WriteAsync(sb.ToString());
        });

        app.Run();
    }
}

}
