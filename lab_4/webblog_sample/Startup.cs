
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
                              IWebHostEnvironment env /* environment, optional */)
        {
            // ** для встановлення компонентів які оброблюють запити використовуються методи обїекта IApplicationBuilder
            // ** об'єкт IWebHostEnvironment дозволяє отримати інформацію про середовище в якому запускається додаток та
            // дозволяє взаємодіяти з ним

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
                endpoints.MapGet("/hw", async context => {
                    await context.Response.WriteAsync("Hello world!");
                });

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
