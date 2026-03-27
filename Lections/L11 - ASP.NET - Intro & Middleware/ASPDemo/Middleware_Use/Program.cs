namespace HelloWorld_5
{
    public class Program
    {
        // ---------------------------------------------------------
        // SAMPLE 1: Middleware Processing with call of Use()
        // ---------------------------------------------------------

        public static void Main_1(string[] args)
        {
            var app = WebApplication.CreateBuilder(args).Build();

            string date = "";

            app.Use(async (context, next) =>
            {
                date = DateTime.Now.ToShortDateString();
                await next.Invoke();
                Console.WriteLine(date);
            });

            app.Run(async (context) => {
                await context.Response.WriteAsync($"Date: {date}");
            });

            app.Run();
        }

        // ---------------------------------------------------------
        // SAMPLE 2: more complex sample uf using Use()
        //   не рекомендовано викликати next.Invoke() якщо вже було
        //   використано WriteAsync, тобто вже почалось сворення відповіді
        // ---------------------------------------------------------

        public static void Main_2(string[] args)
        {
            var app = WebApplication.CreateBuilder(args).Build();

            int x = 0;

            app.Use(async (context, next) => {
                x++;
                await next.Invoke();
                x++;
                context.Response.Headers.ContentType = "text/plain; charset=utf-8";
                await context.Response.WriteAsync($"x={x}");
            });

            app.Use(async (context, next) => {
                x++;
                await next.Invoke();
                x++;
            });

            app.Run(async (context) => {
                x++;
            });

            app.Run();
        }

        // ---------------------------------------------------------
        // SAMPLE 3: next.Invoke() викликати не обов'язково
        // ---------------------------------------------------------

        public static void Main_3(string[] args)
        {
            var app = WebApplication.CreateBuilder(args).Build();

            app.Use(async (context, next) =>
            {
                string? path = context.Request.Path.Value?.ToLower();
                if (path == "/date")
                {
                    await context.Response.WriteAsync($"Use(): Date={DateTime.Now.ToShortDateString()}");
                }
                else
                    await next.Invoke();
            });

            app.Run(async (context) => await context.Response.WriteAsync($"Run()"));
            app.Run();
        }

        // ---------------------------------------------------------
        // SAMPLE 4: usage of named delegates
        // ---------------------------------------------------------

        async static Task GetInfo(HttpContext context)
        {
            await context.Response.WriteAsync($"Run()");
        }

        async static Task GetDate(HttpContext context, Func<Task> next)
        {
            string? path = context.Request.Path.Value?.ToLower();
            if (path == "/date")
            {
                await context.Response.WriteAsync(
                    $"Use(): Date={DateTime.Now.ToShortDateString()}");
            }
            else
                await next.Invoke();
        }

        public static void Main_4(string[] args)
        {
            var app = WebApplication.CreateBuilder(args).Build();

            app.Use(GetDate);
            app.Run(GetInfo);

            app.Run();
        }

        // ---------------------------------------------------------
        // SAMPLE 5: UseWhen & MapWhen
        // ---------------------------------------------------------

        static void HandleBranch(IApplicationBuilder app)
        {
            Console.WriteLine("HandleBranch");
            app.Use(async (context, next) => {
                Console.WriteLine("HandleBranch - Use - before Invoke");
                var branchVer = context.Request.Query["branch"];
                await next.Invoke();
                Console.WriteLine("HandleBranch - Use - after Invoke");
                await context.Response.WriteAsync($"Branch used = {branchVer}");
            });
        }

        public static void Main(string[] args)
        {
            var app = WebApplication.CreateBuilder(args).Build();

            app.UseWhen(context => context.Request.Query.ContainsKey("branch"), HandleBranch);

            app.Use(async (context, next) => {
                Console.WriteLine("Use - before Invoke");
                await next.Invoke();
                Console.WriteLine("Use - after Invoke");
                await context.Response.WriteAsync("Hello from non-Map delegate.");
            });

            app.Run();
        }
    }
}
