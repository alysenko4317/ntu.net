
Тема роботи:
   Взаємодія з СУБД з використанням Entity Framework і технології LINQ to Entities

Під час першого запуску необхідно створити базу даних.
Для цього потрібно виконати міграції. Щоб запустити виконання міграцій, потрібно:
    у меню обрати: Tools -> NuGet Package Manager -> Package Manager Console
    виконати команду: update-database -verbose -context LearningModelContainer

Примітка: якщо Ви отримаєте повідомлення, що update-database — це невідома команда,
      перевірте, що для проєкту встановлено пакет Microsoft.EntityFrameworkCore.Tools
         Tools -> NuGet Package Manager -> Manage NuGet Packages for Solution...
