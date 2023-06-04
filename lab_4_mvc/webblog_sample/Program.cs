using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webblog.Models;
using Microsoft.Extensions.DependencyInjection;

namespace webblog
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // args - command line arguments
            // 
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var context = services.GetRequiredService<BlogContext>();
                    SampleData.Initialize(context);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred seeding the DB.");
                }
            }

            host.Run(); // ���� ������� ���� ������ ������� ������ ��������� �� ������
        }

        // ��� ��������� ������� ASP.NET ������� ��'��� IHost � ������ ����� ����������� �������
        //
        // ��� ��������� IHost ��������������� IHostBuilder ����� Build()
        //
        // ��������� ��'���� ���� ������ ��������� IHostBuilder ������������ ��������� ������ CreateDefaultBuilder
        // � ����� ����������:
        //   - ������������ ���������� �������� (�� ���������� �����)
        //   - ������������ ������������ ����� (�������������� ���� ���������� DOTNET_* �� �������� ���������� �����)
        //   - ���� ���������� ���������
        //   - ������������ ������������ ������� � ����� appsettings.json �� appsettings.{environment}.json + ���� ����������
        //     �� �������� ��������� ����� (environment={development | production})
        //
        // ����� ConfigureWebHostDefaults ������ ������������ �����, � ����:
        //   - ������������� ������������ � ������ ���������� ASPNETCORE_*
        //   - ������� ���-������ Kestrel
        //   - ���� ��������� Host Filtering ���� �������� ����������� ������ ��� ���-�������
        //   - ���� ����� ���������� ASPNETCORE_FORWARDEDHEADERS_ENABLED ����������� � �������� true
        //     �� ���� ��������� Forwarded Headers ���� �������� ��������� �� ������ ��������� X-Forwarded-
        //     (http ������������� ���������)
        //   - ���� ��� ������ ������� IIS �� ����� ����� ��������� ���������� � IIS

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => // �������
                {
                    // here:
                    // �� ��������� ����������� ������� ������� ������ � ��'���� IWebHostBuilder
                    // ����������� ����������� ���-������� ��� ���������� ���-�������
                    //
                    // ����������� ����� ���� �������������� ��������� ���� ������� (�� ����������� �� ����� Startup)
                    webBuilder.UseStartup<Startup>();
                });
    }
}
