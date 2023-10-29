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

        // щоб запустити додаток ASP.NET потрібен об'єкт IHost у рамках якого розгртається додаток
        //
        // для створення IHost використовується IHostBuilder метод Build()
        //
        // створення об'єкта який реалізує інтерфейс IHostBuilder виконуєтьсяза допомогою метода CreateDefaultBuilder
        // а також виконується:
        //   - встановлення кореневого каталогу (де знаходитья проєкт)
        //   - встановлення конфігураціі хоста (завантажуються змінні середовища DOTNET_* та аргументі командного рядка)
        //   - додає провайдери логування
        //   - встановлення конфігураціі додатку з файлів appsettings.json та appsettings.{environment}.json + змінні середовища
        //     та аргументі командноо рядка (environment={development | production})
        //
        // метод ConfigureWebHostDefaults виконує налаштування хоста, а саме:
        //   - завантажується конфігурація зі змінних середовища ASPNETCORE_*
        //   - запускає веб-сервер Kestrel
        //   - додає компонент Host Filtering який дозволяє налаштувати адреси для веб-сервера
        //   - якщо змінна середовища ASPNETCORE_FORWARDEDHEADERS_ENABLED встановлена у значення true
        //     то додає компонент Forwarded Headers який дозволяє считувати із запиту заголовки X-Forwarded-
        //     (http переадресацыя заголовків)
        //   - якщо для роботи потрібен IIS то даний метод забезпечеє інтеграцію з IIS

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => // delegate
                {
                    // here:
                    // за допомогою послідовного виклику ланцюга методів з об'єкту IWebHostBuilder
                    // здійснюється ініціалізація веб-серверу для розгортаня веб-додатку
                    //
                    // викликається метод яким встановлюється стартовий клас додатку (за замовченням це класс Startup)

                    webBuilder.UseStartup<Startup>();
                });
    }
}
