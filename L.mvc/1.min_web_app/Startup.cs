
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using webblog.Services;

namespace minWebApp
{
    public class Startup
    {
        // step 1 - конструктор, якщо він є


        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        //
        // цей метод призначений для реєстраціі сервісів, які використовує додаток
        public void ConfigureServices(IServiceCollection services)  // step 2
        {
            // AddИмяСервиса
            //   С# Методы Розширення - дозволяють додавати нові методи у вже існуючий класс без
            //   створення похідного класу

            // use our own service
            services.AddTransient<IMessageSender, EmailMessageSender>();
            services.AddTransient<MessageService>();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //
        // Цей метод встановлює як саме додаток буде обробляти запити

        //public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IMessageSender messageSender)
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, MessageService messageService)
        {
            // ** для встановлення компонентів які оброблюють запити використовуються методи обїекта IApplicationBuilder
            // ** об'єкт IWebHostEnvironment дозволяє отримати інформацію про середовище в якому запускається додаток та дозволяє взаємодіяти з ним

            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            if (true)
            {
                app.Run(async (context) => {
                    //await context.Response.WriteAsync(messageSender.Send());
                    await context.Response.WriteAsync(messageService.Send());
                });
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>  // анонімний метод, делегат
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }
    }
}
