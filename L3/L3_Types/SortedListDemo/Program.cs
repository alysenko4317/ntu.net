
using System;
using System.Collections.Generic;

class User : IComparable<User>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }

    public User(string firstName, string lastName, int age)
    {
        FirstName = firstName;
        LastName = lastName;
        Age = age;
    }

    // цей метод потрібно перевизначити щоб коректно працювала операція звернення [] до елементу словника
    public override bool Equals(object obj)
    {
        if (obj is User otherUser)
        {
            return FirstName == otherUser.FirstName &&
                   LastName == otherUser.LastName &&
                   Age == otherUser.Age;
        }
        return false;
    }

    // перевизначаємо цей метод оскільки ми хочемо щоб об'єкти з однаковими ім'ям та призвищем вважались "схожими"
    // та відповідно зберігались "поруч", тобто в одному кошику
    public override int GetHashCode()
    {
        return HashCode.Combine(FirstName, LastName);
    }

    public override string ToString()
    {
        return $"{FirstName} {LastName}, Age: {Age}";
    }

    // для сортування елементів необхідно їх якось порівнювати
    public int CompareTo(User? other)
    {
        int r = LastName.CompareTo(other.LastName);
        if (r != 0)
            return r;
        return FirstName.CompareTo(other.FirstName);
    }
}

class Program
{
    static void Main(string[] args)
    {
        //SortedList<User, string> dic = new SortedList<User, string>();
        SortedDictionary<User, string> dic = new SortedDictionary<User, string>();

        dic.Add(new User("Ivan", "Ivanov", 25), "value 1");
        dic.Add(new User("Ivan", "Avesov", 21), "value 2");
        dic.Add(new User("Petro", "Ivanov", 25), "value 2");

        foreach (var kvp in dic)
        {
            Console.WriteLine(kvp.Key);
            //Console.WriteLine(kvp.GetHashCode());
            Console.WriteLine(kvp.Value);
        }
    }
}