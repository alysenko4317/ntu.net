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


    public class Application
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder();
            builder.Configuration.AddJsonFile("person.json");
            // ������������ ��'��� Person �� �������������� � ������������
            builder.Services.Configure<Person>(builder.Configuration);

            var app = builder.Build();

            app.Map("/", (IOptions<Person> options) =>
            {
                Person person = options.Value;  // �������� ��������� ����� Options ��'��� Person
                return person;
            });

            app.Run();
        }
    }
}

