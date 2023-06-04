
using System;
using System.Linq;
using System.Collections.Generic;

namespace SimpleLINQ
{
    class Program
    {
        // *** SAMPLE DATA *** 

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

        // *** MAIN ***

        static void Main(string[] args)
        {
            Console.WriteLine("Простая выборка элементов");
            var q1 = Queries.all(d1);// from x in d1 select x;
            foreach (var x in q1)
                Console.WriteLine("  " + x);

            //----------------------------------------------------------

            Console.WriteLine("Создание нового объекта анонимного типа");
            var q3 = Queries.all_id_value(d1); //from x in d1
                                               //select new { IDENTIFIER = x.id, VALUE = x.value };
            foreach (var x in q3)
                Console.WriteLine("  " + x);
        }
    }
}
