using System;
using System.Diagnostics;
using System.Text;

namespace HelloWorld_5
{
    public class Program
    {
        public static void Main_0(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var app = builder.Build();

            app.MapGet("/", () => "Hello World!");

            app.Run();
        }

        // ---------------------------------------------------------
        // SAMPLE 1: Middleware as inline delegate
        // ---------------------------------------------------------

        public static void Main_1(string[] args)
        {
            var app = WebApplication.CreateBuilder(args).Build();

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello from component!");
            });

            app.Run();
        }

        // ---------------------------------------------------------
        // SAMPLE 2: Middleware as dedicated static method
        // ---------------------------------------------------------

        async static Task HandleRequest(HttpContext context)
        {
            await context.Response.WriteAsync("Hello from component!");
        }

        public static void Main_2(string[] args)
        {
            var app = WebApplication.CreateBuilder(args).Build();
            app.Run(HandleRequest);
            app.Run();
        }

        // ---------------------------------------------------------
        // SAMPLE 3: Middleware as dedicated static method
        // ---------------------------------------------------------

        public static void Main_3(string[] args)
        {
            var app = WebApplication.CreateBuilder(args).Build();

            app.Run(async (context) =>
            {
                StringBuilder responseString = new StringBuilder();

                responseString.AppendLine(DateTime.Now.ToString());
                responseString.AppendLine(context.Request.Method);
                responseString.AppendLine(context.Request.Path);
                responseString.AppendLine(context.Request.Headers.UserAgent);
                responseString.AppendLine("<p></b><a href=\"/index\">index</a></div>");

                if (context.Request.Path == "/")
                {
                    responseString.AppendLine("<div><p>Головна сторінка</div>");
                }
                else if (context.Request.Path == "/index")
                {
                    responseString.AppendLine("<div><p>Index Page</div>");
                }

                Debug.WriteLine(responseString);
                Console.WriteLine(responseString);

                // встановлення одного зі стандартних заголовків
                context.Response.ContentType = "text/html";  // "text/html; charset=utf-8"

                // не всі стандартні заголовки можна встановити безпосередньо через властивості Response
                context.Response.Headers.ContentLanguage = "uk";
                // або можна ще так:
                //    context.Response.Headers["Content-Language"] = "uk";

                // встановлення довільного (користувацького) заголовка
                context.Response.Headers.Append("secret-id", "256");

                context.Response.StatusCode = 200;

                await context.Response.WriteAsync(responseString.ToString());
            });

            app.Run();
        }

        // ---------------------------------------------------------
        // SAMPLE 4: components are created one time for the app
        // ---------------------------------------------------------

        public static void Main_4(string[] args)
        {
            var app = WebApplication.CreateBuilder(args).Build();

            int x = 0;
            app.Run(async (context) =>
            {
                x = x + 1;
                await context.Response.WriteAsync($"Hello from component! Request Number: {x}");
            });

            app.Run();
        }

        // ---------------------------------------------------------
        // SAMPLE 5: list of request headers
        // ---------------------------------------------------------

        public static void Main__(string[] args)
        {
            var app = WebApplication.CreateBuilder(args).Build();

            app.Run(async (context) =>
            {
                context.Response.ContentType = "text/html; charset=utf-8";
                var stringBuilder = new System.Text.StringBuilder("<table>");

                foreach (var header in context.Request.Headers)
                {
                    stringBuilder.Append($"<tr><td>{header.Key}</td><td>{header.Value}</td></tr>");
                }
                stringBuilder.Append("</table>");
                await context.Response.WriteAsync(stringBuilder.ToString());
            });

            app.Run();
        }

        // ---------------------------------------------------------
        // SAMPLE 6: handle different url paths
        // ---------------------------------------------------------

        public static void Main_6(string[] args)
        {
            var app = WebApplication.CreateBuilder(args).Build();

            app.Run(async (context) =>
            {
                var path = context.Request.Path;
                var now = DateTime.Now;
                var response = context.Response;

                if (path == "/date")
                    await response.WriteAsync($"Date: {now.ToShortDateString()}");
                else if (path == "/time")
                    await response.WriteAsync($"Time: {now.ToShortTimeString()}");
                else
                    await response.WriteAsync("Hello!");
            });

            app.Run();
        }

        // ---------------------------------------------------------
        // SAMPLE 7: виведення переліку всіх параметрів з рядка запиту
        //    https://localhost:7124/index?id=5&name=Andrii&count=7
        // ---------------------------------------------------------

        public static void Main_7(string[] args)
        {
            var app = WebApplication.CreateBuilder(args).Build();

            app.Run(async (context) =>
            {
                context.Response.ContentType = "text/html; charset=utf-8";
                var stringBuilder = new System.Text.StringBuilder("<h1>Параметри рядка запиту</h1><table>");
                stringBuilder.Append("<tr><td>Параметр</td><td>Значення</td></tr>");

                foreach (var param in context.Request.Query)
                {
                    stringBuilder.Append($"<tr><td>{param.Key}</td><td>{param.Value}</td></tr>");
                }
                stringBuilder.Append("</table>");
                await context.Response.WriteAsync(stringBuilder.ToString());
            });

            app.Run();
        }

        // ---------------------------------------------------------
        // SAMPLE 8: response with a file or image
        // ---------------------------------------------------------

        public static void Main_(string[] args)
        {
            var app = WebApplication.CreateBuilder(args).Build();

            app.Run(async (context) =>
            {
                var path = context.Request.Path;
                var response = context.Response;

                if (path == "/index")
                {
                    context.Response.ContentType = "text/html; charset=utf-8";
                    await context.Response.SendFileAsync("www/index.html");
                }
                else if (path == "/image")
                    await context.Response.SendFileAsync("www/images/photo.jpg");
                else
                    await response.WriteAsync("Hello!");
            });

            app.Run();
        }

        // ---------------------------------------------------------
        // SAMPLE 9: serving static files (like Apache)
        // ---------------------------------------------------------

        public static void Main(string[] args)
        {
            var app = WebApplication.CreateBuilder(args).Build();

            app.Run(async (context) =>
            {
                var path = context.Request.Path;
                var fullPath = $"www/{path}";
                var response = context.Response;

                response.ContentType = "text/html; charset=utf-8";
                if (File.Exists(fullPath))
                {
                    //context.Response.Headers.ContentDisposition = "attachment; filename=example.jpg";
                    await response.SendFileAsync(fullPath);
                }
                else
                {
                    response.StatusCode = 404;
                    await response.WriteAsync("<h1>Not Found</h1>");
                }
            });

            app.Run();
        }
    }
}
