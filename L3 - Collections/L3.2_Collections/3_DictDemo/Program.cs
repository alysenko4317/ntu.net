
using System;
using System.Collections.Generic;

class User
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
    public override int GetHashCode() {
        Console.WriteLine("  -- run GetHashCode() ");
        //return base.GetHashCode();
        //return LastName.GetHashCode();
        //return LastName.GetHashCode() + FirstName.GetHashCode();
        //   або можна використати XOR: LastName.GetHashCode() ^ FirstName.GetHashCode();
        return HashCode.Combine(FirstName, LastName);
    }
    
    public override string ToString() {
        return $"{FirstName} {LastName}, Age: {Age}";
    }
}

class Program
{
    static void Main(string[] args)
    {
        Dictionary<User, string> dic = new Dictionary<User, string>();

        dic.Add(new User("Ivan", "Ivanov", 25), "value 1");
        dic.Add(new User("Ivan", "Ivanov", 21), "value 2");
        dic.Add(new User("Petro", "Ivanov", 25), "value 2");

        if (dic.ContainsKey(new User("Ivan", "Ivanov", 25)))
        {
            var value = dic[new User("Ivan", "Ivanov", 25)];
            Console.WriteLine("value=" + value);
        }

        foreach (var kvp in dic)
        {
            Console.WriteLine(kvp.Key);
            //Console.WriteLine(kvp.GetHashCode());
            Console.WriteLine(kvp.Value);
        }
    }
}
