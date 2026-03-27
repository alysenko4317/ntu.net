using Microsoft.Extensions.Options;

// "Паттерн Options"(або "шаблон опцій") у контексті програмування — це підхід до конфігурації програми,
//     який дозволяє легко управляти і внести зміни до налаштувань додатка без необхідності зміни коду.
//     Цей паттерн часто використовується в .NET Core для управління налаштуваннями, які можуть бути
//     зчитані з файлів конфігурації, таких як appsettings.json, змінних оточення, аргументів командного рядка тощо.
//
// Користування паттерном Options у .NET Core включає в себе кілька ключових компонентів:
//
//    Класи опцій(Options classes): Визначають властивості, які представляють групу пов’язаних налаштувань.
//    Кожен клас конфігурації відображається на секцію в файлі appsettings.json або іншому джерелі конфігурації.
//
//    Реєстрація опцій (Registration of options): В Startup класі або при налаштуванні хоста, ви реєструєте
//    ці класи опцій за допомогою методів розширення, таких як services.Configure<>(), що дозволяє внедрювати
//    інстанси через конструктори компонентів, які їх використовують.
//
//    Впровадження опцій (Injection of options): Ви можете отримувати конфігуровані опції через залежності,
//    використовуючи IOptions<T>, IOptionsSnapshot<T>, або IOptionsMonitor<T>, де T — це тип вашого класу конфігурації.
//
//    Динамічні оновлення (Dynamic updates): IOptionsSnapshot і IOptionsMonitor дозволяють динамічно реагувати на
//    зміни в налаштуваннях конфігурації без необхідності перезавантаження додатка.
//
// Цей паттерн є дуже корисним у сучасних програмах, де є потреба в гнучкості і можливості швидко адаптувати
// поведінку додатка до змін у його середовищі чи вимогах бізнесу без зайвих затрат часу на деплоймент нових версій.


namespace App
{
    public class Person
    {
        public string Name { get; set; } = "";
        public int Age { get; set; }
        public List<string> Languages { get; set; } = new();
        public Company? Company { get; set; }
    }

    public class Company
    {
        public string Title { get; set; } = "";
        public string Country { get; set; } = "";
    }


    public class Application
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder();
            builder.Configuration.AddJsonFile("person.json");
            // встановлюємо об'єкт Person за налаштуваннями з конфігурації
            builder.Services.Configure<Person>(builder.Configuration);

            var app = builder.Build();

            app.Map("/", (IOptions<Person> options) =>
            {
                Person person = options.Value;  // отримуємо переданий через Options об'єкт Person
                return person;
            });

            app.Run();
        }
    }
}

