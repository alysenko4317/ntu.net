
using System;
using System.Linq;
using System.Collections.Generic;

namespace SimpleLINQ {

class Program
{
    public class Data         // класс даних
    {
        public int id;        // ключ
        public string grp;    // для групування
        public string value;  // значення

        public Data(int i, string g, string v)   // конструктор
        {
            this.id = i;
            this.grp = g;
            this.value = v;
        }

        public override string ToString() {     // приведення до рядка
            return "(id=" + this.id.ToString() + "; grp=" + this.grp + "; value=" + this.value + ")";
        }
    }

    // клас для порівняння даних
    public class DataEqualityComparer : IEqualityComparer<Data>   
    {
        public bool Equals(Data x, Data y) {
            return (x.id == y.id && x.grp == y.grp && x.value == y.value);
        }

        public int GetHashCode(Data obj) {
            return obj.id;
        }
    }

    public class DataLink    // зв'язок між списками
    {
        public int d1;
        public int d2;
        public DataLink(int i1, int i2) {
            this.d1 = i1;
            this.d2 = i2;
        }
    }

    // *** ТЕСТОВІ ДАНІ *** 

    static List<Data> d1 = new List<Data>()
    {
        new Data(1, "group1", "11"),
        new Data(2, "group1", "12"),
        new Data(3, "group2", "13"),
        new Data(5, "group2", "15")
    };

    static List<Data> d2 = new List<Data>()
    {
        new Data(1, "group2", "21"),
        new Data(2, "group3", "221"),
        new Data(2, "group3", "222"),
        new Data(4, "group3", "24")
    };

    static List<Data> d1_for_distinct = new List<Data>()
    {
        new Data(1, "group1", "11"),
        new Data(1, "group1", "11"),
        new Data(1, "group1", "11"),
        new Data(2, "group1", "12"),
        new Data(2, "group1", "12")
    };

    static List<DataLink> lnk = new List<DataLink>()
    {
        new DataLink(1,1),
        new DataLink(1,2),
        new DataLink(1,4),
        new DataLink(2,1),
        new DataLink(2,2),
        new DataLink(2,4),
        new DataLink(5,1),
        new DataLink(5,2)
    };

    // *** ОСНОВНА ЧАСТИНА  ***

    static void Main(string[] args)
    {
        Console.WriteLine("Звичайна вибірка елементів");
        var q1 = from x in d1 select x;
        foreach (var x in q1)
            Console.WriteLine("  " + x);

        //----------------------------------------------------------

        Console.WriteLine("Вибірка окремого поля (проекція)");
        var q2 = from x in d1 select x.value;
        foreach (var x in q2)
            Console.WriteLine("  " + x);

        //----------------------------------------------------------

        Console.WriteLine("Створення нового об'єкту анонімного типу");
        var q3 = from x in d1
                    select new { IDENTIFIER = x.id, VALUE = x.value };
        foreach (var x in q3)
            Console.WriteLine("  " + x);

        //##########################################################

        Console.WriteLine("Умови");

        var q4 = from x in d1
                    where x.id > 1 && (x.grp == "group1" || x.grp == "group2")
                    select x;
            foreach (var x in q4)
                Console.WriteLine("  " + x);

        //----------------------------------------------------------

        Console.WriteLine("Вибірка за типом значення");
        object[] array = new object[] { 123, "строка 1", true, "строка 2" };
        var qo = from x in array.OfType<string>()
                    select x;
        foreach (var x in qo)
            Console.WriteLine("  " + x);

        //##########################################################

        Console.WriteLine("Сортування");
        var q5 = from x in d1
                    where x.id > 1 && (x.grp == "group1" || x.grp == "group2")
                    orderby x.grp descending, x.id descending
                    select x;
        foreach (var x in q5)
            Console.WriteLine("  " + x);

        //----------------------------------------------------------

        Console.WriteLine("Сортування (з використанням методів)");
        var q51 = d1.Where((x) => {
                            return x.id > 1 && (x.grp == "group1" || x.grp == "group2");
                        })
                    .OrderByDescending(x => x.grp)
                    .ThenByDescending(x => x.id);
        foreach (var x in q51)
            Console.WriteLine("  " + x);

        //##########################################################

        Console.WriteLine("Посторінкова видача даних (оператори розбивки)");
        var qp = GetPage(d1, 2, 2);
        foreach (var x in qp)
            Console.WriteLine("  " + x);

        //----------------------------------------------------------

        Console.WriteLine("Використання SkipWhile та TakeWhile");
        int[] intArray = new int[] { 1, 2, 3, 4, 5, 6, 7, 8 };
        var qw = intArray.SkipWhile(x => (x < 4)).TakeWhile(x => x <= 7);
        foreach (var x in qw)
            Console.WriteLine("  " + x);

        //##########################################################

        Console.WriteLine("Декартів добуток");
        var q6 = from x in d1
                    from y in d2
                    select new { v1 = x.value, v2 = y.value };
        foreach (var x in q6)
            Console.WriteLine("  " + x);

        //----------------------------------------------------------

        Console.WriteLine("Inner Join з використанням Where");
        var q7 = from x in d1
                    from y in d2
                    where x.id == y.id
                    select new { v1 = x.value, v2 = y.value };
        foreach (var x in q7)
            Console.WriteLine("  " + x);

        //----------------------------------------------------------

        Console.WriteLine("Cross Join (Inner Join) з використанням Join");
        var q8 = from x in d1
                    join y in d2 on x.id equals y.id
                    select new { v1 = x.value, v2 = y.value };
        foreach (var x in q8)
            Console.WriteLine("  " + x);

        //----------------------------------------------------------

        Console.WriteLine("Cross Join (збереження об'єкта)");
        var q9 = from x in d1
                    join y in d2 on x.id equals y.id
                    select new { v1 = x.value, d2Group = y };
        foreach (var x in q9)
            Console.WriteLine("  " + x);

        //----------------------------------------------------------

        // Обираються усі елементи з d1 якщо є пов'язані з d2 (outer join).
        // У temp записується вся група, її елементи можна перебирати окремо.

        Console.WriteLine("Group Join");
        var q10 = from x in d1
                    join y in d2 on x.id equals y.id into temp
                    select new { v1 = x.value, d2Group = temp };
        foreach (var x in q10)
        {
            Console.WriteLine("  " + x.v1);
            foreach (var y in x.d2Group)
                Console.WriteLine("   " + y);
        }

        //----------------------------------------------------------

        Console.WriteLine("Cross Join та Group Join");
        var q11 = from x in d1
                    join y in d2 on x.id equals y.id into temp
                    from t in temp
                    select new { v1 = x.value, v2 = t.value, cnt = temp.Count() };
        foreach (var x in q11)
            Console.WriteLine("  " + x);

        //----------------------------------------------------------

        Console.WriteLine("Outer Join");
        var q12 = from x in d1
                    join y in d2 on x.id equals y.id into temp
                    from t in temp.DefaultIfEmpty()
                    select new { v1 = x.value, v2 = ((t == null) ? "null" : t.value) };
        foreach (var x in q12)
            Console.WriteLine("  " + x);

        //----------------------------------------------------------

        Console.WriteLine("Використання Join для складних ключів");
        var q12_1 = from x in d1
                    join y in d1_for_distinct on new { x.id, x.grp } equals new { y.id, y.grp } into details
                    from d in details
                    select d;
        foreach (var x in q12_1)
            Console.WriteLine(x);

        //##########################################################

        //Действия над множествами
        Console.WriteLine("Distinct - Значення, що не повторюються");
        var q13 = (from x in d1 select x.grp).Distinct();
        foreach (var x in q13)
            Console.WriteLine("  " + x);

        //----------------------------------------------------------

        Console.WriteLine("Distinct - Значення, які повторюються для об'єктів");
        var q14 = (from x in d1_for_distinct select x).Distinct();
        foreach (var x in q14)
            Console.WriteLine("  " + x);

        //----------------------------------------------------------

        Console.WriteLine("Distinct - Значення, що не повторюються для об'єктів");
        var q15 = (from x in d1_for_distinct select x).Distinct(new DataEqualityComparer());
        foreach (var x in q15)
            Console.WriteLine("  " + x);

        //----------------------------------------------------------

        Console.WriteLine("Union - Об'єднання з виключенням дублікатів");
        int[] i1 = new int[] { 1, 2, 3, 4 };
        int[] i1_1 = new int[] { 2, 3, 4, 1 };
        int[] i2 = new int[] { 2, 3, 4, 5 };
        foreach (var x in i1.Union(i2))
            Console.WriteLine("  " + x);

        Console.WriteLine("Union - Об'єднання для об'єктів");
        foreach (var x in d1.Union(d1_for_distinct))
            Console.WriteLine("  " + x);

        Console.WriteLine("Union - Об'єднання для об'єктів з виключенням дублікатів 1");
        foreach (var x in d1.Union(d1_for_distinct, new DataEqualityComparer()))
            Console.WriteLine("  " + x);

        Console.WriteLine("Union - Об'єднання для об'єктів з виключенням дублікатів 2");
        foreach (var x in d1.Union(d1_for_distinct).Union(d2).Distinct(new DataEqualityComparer()))
            Console.WriteLine("  " + x);

        //----------------------------------------------------------

        Console.WriteLine("Concat - Об'єднання без виключення дублікатів");
        foreach (var x in i1.Concat(i2))
            Console.WriteLine("  " + x);

        Console.WriteLine("SequenceEqual - Перевірка збігу елементів та порядку їх слідування");
        Console.WriteLine(i1.SequenceEqual(i1));
        Console.WriteLine(i1.SequenceEqual(i2));

        //----------------------------------------------------------

        Console.WriteLine("Intersect - Перетин множин");
        foreach (var x in i1.Intersect(i2))
            Console.WriteLine("  " + x);

        Console.WriteLine("Intersect - Перетин множин для об'єктів");
        foreach (var x in d1.Intersect(d1_for_distinct, new DataEqualityComparer()))
            Console.WriteLine("  " + x);

        //----------------------------------------------------------

        Console.WriteLine("Except - Різниця множин");
        foreach (var x in i1.Except(i2))
            Console.WriteLine("  " + x);

        Console.WriteLine("Except - Різниця множин для об'єктів");
        foreach (var x in d1.Except(d1_for_distinct, new DataEqualityComparer()))
            Console.WriteLine("  " + x);

        //##########################################################

        Console.WriteLine("Функції агрегування");
        Console.WriteLine("  Count - кількість елементів");
        Console.WriteLine("    " + d1.Count());
        Console.WriteLine("  Count з умовою");
        Console.WriteLine("    " + d1.Count(x => x.id > 1));

        // Могут использоваться также следующие агрегирующие функции
        //   Sum - сума елементів
        //   Min - мінімальний елемент
        //   Max - максимальний елемент
        //   Average - середнє аріфметичне значення

        //----------------------------------------------------------

        Console.WriteLine("Aggregate - Агрегування значень");
        var qa1 = d1.Aggregate(new Data(0, "", ""),
            (total, next) =>
            {
                if (next.id > 1) total.id += next.id;
                return total;
            });

        Console.WriteLine("  " + qa1);

        //##########################################################

        Console.WriteLine("Групування");
        var q16 = from x in d1.Union(d2)
                    group x by x.grp into g
                    select new { Key = g.Key, Values = g };
        foreach (var x in q16)
        {
            Console.WriteLine("  " + x.Key);
            foreach (var y in x.Values)
                Console.WriteLine("   " + y);
        }

        //----------------------------------------------------------

        Console.WriteLine("Групування з функціями агрегування");
        var q17 = from x in d1.Union(d2)
                    group x by x.grp into g
                    select new { Key = g.Key,
                                Values = g,
                                cnt = g.Count(),
                                cnt1 = g.Count(x => x.id > 1),
                                sum = g.Sum(x => x.id),
                                min = g.Min(x => x.id) };

        foreach (var x in q17)
        {
            Console.WriteLine("  " + x);
            foreach (var y in x.Values)
                    Console.WriteLine("   " + y);
        }

        //----------------------------------------------------------

        Console.WriteLine("Групування - Any");
        var q18 = from x in d1.Union(d2)
                    group x by x.grp into g
                    where g.Any(x => x.id > 3)
                    select new { Key = g.Key, Values = g };

        foreach (var x in q18)
        {
            Console.WriteLine("  " + x.Key);
            foreach (var y in x.Values)
                Console.WriteLine("   " + y);
        }

        Console.WriteLine("Групування - All");
        var q19 = from x in d1.Union(d2)
                    group x by x.grp into g
                    where g.All(x => x.id > 1)
                    select new { Key = g.Key, Values = g };

        foreach (var x in q19)
        {
            Console.WriteLine("  " + x.Key);
            foreach (var y in x.Values)
                Console.WriteLine("   " + y);
        }

        //----------------------------------------------------------

        Console.WriteLine("Імітація зв'язку many-to-many");
        var lnk1 = from x in d1
                    join l in lnk on x.id equals l.d1 into temp
                    from t1 in temp
                    join y in d2 on t1.d2 equals y.id into temp2
                    from t2 in temp2
                    select new { id1 = x.id, id2 = t2.id };

        foreach (var x in lnk1)
            Console.WriteLine("  " + x);

        //----------------------------------------------------------

        Console.WriteLine("Імітація зв'язку many-to-many з перевіркою умови");
        var lnk2 = from x in d1
                    join l in lnk on x.id equals l.d1 into temp
                    from t1 in temp
                    join y in d2 on t1.d2 equals y.id into temp2
                    where temp2.Any(t => t.value == "24")
                    select x;

        foreach (var x in lnk2)
            Console.WriteLine("  " + x);

        //----------------------------------------------------------

        Console.WriteLine("Імітація зв'язку many-to-many, використання let, перевірка умови");
        var lnk3 = from x in d1
                    let temp1 = from l in lnk where l.d1 == x.id select l
                    from t1 in temp1
                    let temp2 = from y in d2
                                where y.id == t1.d2 && y.value == "24"
                                select y where temp2.Count() > 0
                    //let temp2 = from y in d2 where y.id == t1.d2
                    // select y
                    //where temp2.Any(t=>t.value == "24")
                    select x;

        foreach (var x in lnk3)
            Console.WriteLine("  " + x);

        //##########################################################

        Console.WriteLine("Deferred Execution - Відкладене виконання запиту");
        var e1 = from x in d1 select x;
        Console.WriteLine(e1.GetType().Name);
        foreach (var x in e1)
            Console.WriteLine("  " + x);

        Console.WriteLine("При зміні джерела даних запит видає нові результати");
        d1.Add(new Data(333, "", ""));
        foreach (var x in e1)
            Console.WriteLine("  " + x);

        //----------------------------------------------------------

        Console.WriteLine("Immediate Execution - Негайне виконання запиту, результат перетворюється у список");
        var e2 = (from x in d1 select x).ToList();
        Console.WriteLine(e2.GetType().Name);
        foreach (var x in e2)
            Console.WriteLine("  " + x);

        Console.WriteLine("Результат перетворюється у масив");
        var e3 = (from x in d1 select x).ToArray();
        Console.WriteLine(e3.GetType().Name);
        foreach (var x in e3)
            Console.WriteLine("  " + x);

        Console.WriteLine("Результат перетворюється у словник");
        var e4 = (from x in d1 select x).ToDictionary(x => x.id);
        Console.WriteLine(e4.GetType().Name);
        foreach (var x in e4)
            Console.WriteLine("  " + x);

        Console.WriteLine("Результат перетворюється у Lookup");
        var e5 = (from x in d1_for_distinct select x).ToLookup(x => x.id);
        Console.WriteLine(e5.GetType().Name);
        foreach (var x in e5)
        {
            Console.WriteLine("  " + x.Key);
            foreach (var y in x)
                Console.WriteLine("   " + y);
        }

        //##########################################################

        Console.WriteLine("Отримання першого елемента з вибірки");
        var f1 = (from x in d2 select x).First(x => x.id == 2);
        Console.WriteLine("  " + f1);

        Console.WriteLine("Отримання першого елемента з вибірки або значення за замовчуванням");
        var f2 = (from x in d2 select x).FirstOrDefault(x => x.id == 22);
        Console.WriteLine("  " + (f2 == null ? "null" : f2.ToString()));

        Console.WriteLine("Отримання елемента у заданій позиції");
        var f3 = (from x in d2 select x).ElementAt(2);
        Console.WriteLine("  " + f3);

        //##########################################################

        Console.WriteLine("Генерація послідовностей");
        Console.WriteLine("  Range");
        foreach (var x in Enumerable.Range(1, 5))
            Console.WriteLine("    " + x);
        Console.WriteLine("  Repeat");
        foreach (var x in Enumerable.Repeat<int>(10, 3))
            Console.WriteLine("    " + x);

        Console.ReadLine();  // залишаємо вікно консолі відкритим
    }

    // отримання потрібної сторінки даних
    static List<Data> GetPage(List<Data> data, int pageNum, int pageSize)
    {
        // кількість перепускаємих елементів
        int skipSize = (pageNum - 1) * pageSize;
        var q = data.OrderBy(x => x.id).Skip(skipSize).Take(pageSize);
        return q.ToList();
    }
}
}
