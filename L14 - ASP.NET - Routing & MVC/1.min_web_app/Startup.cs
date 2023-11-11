
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
        // step 1 - �����������, ���� �� �


        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        //
        // ��� ����� ����������� ��� ��������� ������, �� ����������� �������
        public void ConfigureServices(IServiceCollection services)  // step 2
        {
            // Add����������
            //   �# ������ ���������� - ���������� �������� ��� ������ � ��� �������� ����� ���
            //   ��������� ��������� �����

            // use our own service
            services.AddTransient<IMessageSender, EmailMessageSender>();
            services.AddTransient<MessageService>();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //
        // ��� ����� ���������� �� ���� ������� ���� ��������� ������

        //public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IMessageSender messageSender)
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, MessageService messageService)
        {
            // ** ��� ������������ ���������� �� ���������� ������ ���������������� ������ ������ IApplicationBuilder
            // ** ��'��� IWebHostEnvironment �������� �������� ���������� ��� ���������� � ����� ����������� ������� �� �������� ��������� � ���

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
                endpoints.MapGet("/", async context =>  // �������� �����, �������
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }
    }
}
