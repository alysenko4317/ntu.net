using System;
using System.Collections.Generic;
using System.Linq;

public class Person
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }
}

class Program
{
    static void Main()
    {
        List<Person> people = new List<Person>
        {
            new Person { FirstName = "Іван", LastName = "Петров", Age = 30 },
            new Person { FirstName = "Марія", LastName = "Іванова", Age = 25 },
            new Person { FirstName = "Петро", LastName = "Сидоров", Age = 35 }
        };

        // Використання анонімного типу для проектування результату LINQ запиту
        var result = from person in people
                     select new
                     {
                         FullName = $"{person.FirstName} {person.LastName}",
                         person.Age
                     };
        string a = null;
        // Вивід результату
        foreach (var personInfo in result)
        {
            Console.WriteLine($"Повне ім'я: {personInfo.FullName}, Вік: {personInfo.Age} років");
        }
    }
}
