
Тема работы:
   Взаимодействие с СУБД с использованием Entity Framework и технологии LINQ to Entities

При первом запуске необходимо создать базу данных.
Для этого необходимо выполнить миграции. Чтобы запустить выполенние миграций необходимо:
    в меню выбрать: Tools -> NuGet Package Manager -> Package Manager Console
    выполнить команду: update-database -verbose -context LearningModelContainer
       Note: если Вы получите сообщение о том что это неизвестная команда, проверьте
             что для проекта установлен пакет Microsoft.EntityFrameworkCore.Tools
                - Tools -> NuGet Package Manager -> Manage NuGet Packages for Solution...
