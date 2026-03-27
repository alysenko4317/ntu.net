
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using webblog.Models;   // пространство имен моделей
using Microsoft.EntityFrameworkCore; // пространство имен EntityFramework
using System.Linq;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using webblog.Services;

namespace webblog
{
    public class Startup
    {
        // constructor is optional (called on first step)
        public Startup(IConfiguration configuration)  
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the DI container.
        // IServiceCollection contains ~90 services by default
        // цей метод призначений для реєстраціі сервісів, які використовує додаток
        public void ConfigureServices(IServiceCollection services)  // this method is also optional (called on second step)
        {
            // enumerate all available services to the VS Debug Output window
            var list = services.ToList();
            Debug.WriteLine("************** ConfigureServices ****************** ");
            foreach (var srv in list)
                Debug.WriteLine(srv.ToString());

            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<BlogContext>(options => options.UseSqlServer(connection));
            services.AddControllersWithViews();

            // AddИмяСервиса
            //   С# Методы Розширення - дозволяють додавати нові методи у вже існуючий класс без
            //   створення похідного класу

            // use our own service
            services.AddTransient<IMessageSender, EmailMessageSender>();
        }

        // --------------------------------------------------- // delegates for app.Map Sample

        private static void Index(IApplicationBuilder app)
        {
            app.Run(async (context) => {
                await context.Response.WriteAsync("Index");
            });
        }

        private static void About(IApplicationBuilder app)
        {
            app.Run(async (context) => {
                await context.Response.WriteAsync("About");
            });
        }

        private static void handleId(IApplicationBuilder app)
        {
            app.Run(async (context) => {
                await context.Response.WriteAsync("id is equal to 5");
            });
        }

        // --------------------------------------------------- // delegates for Map


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // Цей метод встановлює як саме додаток буде обробляти запити
        public void Configure(IApplicationBuilder app /* builder, required */,
                              IWebHostEnvironment env /* environment, optional */,
                              IMessageSender messsageSender)  // required method (called on the third step)
        {
            // ** для встановлення компонентів які оброблюють запити використовуються методи обїекта IApplicationBuilder
            // ** об'єкт IWebHostEnvironment дозволяє отримати інформацію про середовище в якому запускається додаток та дозволяє взаємодіяти з ним

            // --------------------------------------------------- // sample of using Run

            if (false)
            {
                app.Run(async (context) => {
                    await context.Response.WriteAsync(messsageSender.Send());
                });
            }

            // --------------------------------------------------- // sample of using Run

            if (false)  
            {
                int x = 2;
                // запуск анонимного методу (наш особисто створений middleware-компонент)
                // Run не підтримує пердачу керування далі по ланцюжку, тому якщо це потрібно треба вікористовувати Use
                // about return value of async methods:
                //     https://stackoverflow.com/questions/25191512/async-await-return-task
                app.Run(async (context) => {
                    x = x * 2;
                    await context.Response.WriteAsync($"Result: {x}");
                });
            }

            // --------------------------------------------------- // sample of using Use

            if (false)  
            {
                int x = 5;
                int y = 8;
                int z = 0;

                app.Use(async (context, next) => {
                    z = x * y;
                    await next.Invoke();
                });

                app.Run(async (context) => {
                    await context.Response.WriteAsync($"x * y =  {z}");
                });
            }

            // --------------------------------------------------- // sample of using Use (complex)

            if (false)  // sample of using Use
            {
                int x = 2;

                app.Use(async (context, next) => {
                    x = x * 2;
                    await next.Invoke();
                    x = x * 2;
                    await context.Response.WriteAsync($"result_1:{x}");
                });

                app.Use(async (context, next) => {
                    x = x * 2;
                    await next.Invoke();
                    x = x * 2;
                    await context.Response.WriteAsync($"result_2:{x}");
                });

                app.Run(async (context) => {
                    x = x * 2;
                    await Task.FromResult(0);
                });
            }

            // --------------------------------------------------- // sample of using Map

            if (false)
            {
                app.Map("/index", Index);
                app.Map("/about", About);
                app.Run(async (context) => {
                    await context.Response.WriteAsync("Page not found!");
                });
            }

            // --------------------------------------------------- // sample of using Map (with includes)

            if (false)
            {
                app.Map("/home", home => {
                    home.Map("/index", Index);
                });

                app.Map("/about", About);

                app.Run(async (context) => {
                    await context.Response.WriteAsync("Page not found!");
                });
            }

            // --------------------------------------------------- // sample of using MapWhen (with includes)

            if (false)
            {
                app.MapWhen(context => {
                    bool a = context.Request.Query.ContainsKey("id");
                    var b = context.Request.Query["id"];
                    return a && b == "5";
                }, handleId);

                //app => {
                //    app.Run(async (context) => {
                //        await context.Response.WriteAsync("Good bye...");
                //    });

                    app.Run(async (context) => {
                    await context.Response.WriteAsync("Good bye...");
                });
            }

            // --------------------------------------------------- // END of SAMPLES

            if (env.IsDevelopment())  // we can check here for development or production environment
            {
                // виведення розширенної інформаціі про помилки
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            // включеє маршрутизацію url-адрес, безпосередньо налаштування endpoints див. нижче по коду
            app.UseRouting();

            app.UseAuthorization();

            // обробка кожного маршруту
            app.UseEndpoints(endpoints =>
            {
                // ось та можна добавити новый endpoint
                endpoints.MapGet("/hw", async context =>
                {
                    await context.Response.WriteAsync("Hello world!");
                });

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
