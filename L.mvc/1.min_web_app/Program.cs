using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace minWebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Debug.WriteLine("******* MAIN - BEGIN");
            CreateHostBuilder(args).Build().Run();
            Debug.WriteLine("******* MAIN - END");
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
                .ConfigureWebHostDefaults(webBuilder => // delegate
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
