namespace HelloWorld_5
{
    public class Program
    {
        // ---------------------------------------------------------
        // SAMPLE 1: Form Processing
        // ---------------------------------------------------------

        public static void Main(string[] args)
        {
            var app = WebApplication.CreateBuilder(args).Build();

            app.Run(async (context) =>
            {
                context.Response.ContentType = "text/html; charset=utf-8";

                if (context.Request.Path == "/postuser")
                {
                    var form = context.Request.Form;
                    string name = form["name"];
                    string age = form["age"];
                    await context.Response.WriteAsync($"<div><p>Name: {name}</p><p>Age: {age}</p></div>");
                }
                else
                {
                    await context.Response.SendFileAsync("wwwroot/index.html");
                }
            });

            app.Run();
        }

        // ---------------------------------------------------------
        // SAMPLE 2: Form with multiple fields with the same name
        // ---------------------------------------------------------

        public static void Main_2(string[] args)
        {
            var app = WebApplication.CreateBuilder(args).Build();

            app.Run(async (context) =>
            {
                context.Response.ContentType = "text/html; charset=utf-8";

                if (context.Request.Path == "/postuser")
                {
                    var form = context.Request.Form;
                    string name = form["name"];
                    string age = form["age"];
                    string[] languages = form["languages"];

                    string langList = "";
                    foreach (var lang in languages) {
                        langList += $"<li>{lang.Trim()}</li>";
                    }

                    await context.Response.WriteAsync($"<div><p>Name: {name}</p>" +
                                                      $"<p>Age: {age}</p>" +
                                                      $"<div>Languages:<ul>{langList}</ul></div>");
                }
                else
                {
                    await context.Response.SendFileAsync("wwwroot/lang.html");
                }
            });

            app.Run();
        }

        // ---------------------------------------------------------
        // SAMPLE 3: Redirect
        // ---------------------------------------------------------

        public static void Main_3(string[] args)
        {
            var app = WebApplication.CreateBuilder(args).Build();

            app.Run(async (context) =>
            {
                if (context.Request.Path == "/old")
                    context.Response.Redirect("/new");
                else if (context.Request.Path == "/new")
                    await context.Response.WriteAsync("New Page");
                else
                    await context.Response.WriteAsync("Main Page");
            });

            app.Run();
        }

        // ---------------------------------------------------------
        // SAMPLE 4: File upload
        // ---------------------------------------------------------

        public static void Main_4(string[] args)
        {
            var app = WebApplication.CreateBuilder(args).Build();

            app.Run(async (context) =>
            {
                var response = context.Response;
                var request = context.Request;

                response.ContentType = "text/html; charset=utf-8";

                if (request.Path == "/upload" && request.Method == "POST")
                {
                    IFormFileCollection files = request.Form.Files;
                    var uploadPath = $"{Directory.GetCurrentDirectory()}/wwwroot/uploads";
                    Directory.CreateDirectory(uploadPath);

                    foreach (var file in files)
                    {
                        string fullPath = $"{uploadPath}/{file.FileName}";

                        using (var fileStream = new FileStream(fullPath, FileMode.Create)) {
                            await file.CopyToAsync(fileStream);
                        }
                    }
                    await response.WriteAsync("файли успішно завантажені");
                }
                else
                    await response.SendFileAsync("wwwroot/upload.html");
            });

            app.Run();
        }
    }
}