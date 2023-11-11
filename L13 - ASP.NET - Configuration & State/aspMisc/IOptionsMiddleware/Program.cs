using Microsoft.Extensions.Options;

// "������� Options"(��� "������ �����") � �������� ������������� � �� ����� �� ������������ ��������,
//     ���� �������� ����� ��������� � ������ ���� �� ����������� ������� ��� ����������� ���� ����.
//     ��� ������� ����� ��������������� � .NET Core ��� ��������� ��������������, �� ������ ����
//     ������ � ����� ������������, ����� �� appsettings.json, ������ ��������, ��������� ���������� ����� ����.
//
// ������������ ��������� Options � .NET Core ������ � ���� ����� �������� ����������:
//
//    ����� �����(Options classes): ���������� ����������, �� ������������� ����� ��������� �����������.
//    ����� ���� ������������ ������������ �� ������ � ���� appsettings.json ��� ������ ������ ������������.
//
//    ��������� ����� (Registration of options): � Startup ���� ��� ��� ����������� �����, �� ��������
//    �� ����� ����� �� ��������� ������ ����������, ����� �� services.Configure<>(), �� �������� ����������
//    �������� ����� ������������ ����������, �� �� ��������������.
//
//    ������������ ����� (Injection of options): �� ������ ���������� ������������ ����� ����� ���������,
//    �������������� IOptions<T>, IOptionsSnapshot<T>, ��� IOptionsMonitor<T>, �� T � �� ��� ������ ����� ������������.
//
//    ������� ��������� (Dynamic updates): IOptionsSnapshot � IOptionsMonitor ���������� �������� ��������� ��
//    ���� � ������������� ������������ ��� ����������� ���������������� �������.
//
// ��� ������� � ���� �������� � �������� ���������, �� � ������� � �������� � ��������� ������ ����������
// �������� ������� �� ��� � ���� ���������� �� ������� ������ ��� ������ ������ ���� �� ���������� ����� �����.


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

    public class PersonMiddleware
    {
        private readonly RequestDelegate _next;
        public Person Person { get; }

        public PersonMiddleware(RequestDelegate next, IOptions<Person> options)
        {
            _next = next;
            Person = options.Value;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            System.Text.StringBuilder stringBuilder = new();
            stringBuilder.Append($"<p>Name: {Person.Name}</p>");
            stringBuilder.Append($"<p>Age: {Person.Age}</p>");
            stringBuilder.Append($"<p>Company: {Person.Company?.Title}</p>");
            stringBuilder.Append("<h3>Languages</h3><ul>");
            foreach (string lang in Person.Languages)
                stringBuilder.Append($"<li>{lang}</li>");
            stringBuilder.Append("</ul>");
            await context.Response.WriteAsync(stringBuilder.ToString());
        }
    }

    public class Application
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder();
            builder.Configuration.AddJsonFile("person.json");

            builder.Services.Configure<Person>(builder.Configuration);
            builder.Services.Configure<Person>(opt =>
            {
                opt.Age = 22;
            });

            var app = builder.Build();

            app.UseMiddleware<PersonMiddleware>();
            app.Run();
        }
    }
}

