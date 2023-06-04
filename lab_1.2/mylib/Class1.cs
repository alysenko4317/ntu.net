
using System.Linq;
using System.Collections.Generic;

namespace SimpleLINQ
{
    public class Data         // Класс данных
    {
        public int id;        // Ключ
        public string grp;    // Для группировки
        public string value;  // Значение

        public Data(int i, string g, string v)   // Конструктор
        {
            this.id = i;
            this.grp = g;
            this.value = v;
        }

        public override string ToString()  // Приведение к строке
        {
            return "(id=" + this.id.ToString() + "; grp=" + this.grp + "; value=" + this.value + ")";
        }
    }

    public class DataEqualityComparer : IEqualityComparer<Data>   // Класс для сравнения данных
    {
        public bool Equals(Data x, Data y)
        {
            return (x.id == y.id && x.grp == y.grp && x.value == y.value);
        }
        public int GetHashCode(Data obj)
        {
            return obj.id;
        }
    }

    public class DataLink    // Связь между списками
    {
        public int d1;
        public int d2;
        public DataLink(int i1, int i2)
        {
            this.d1 = i1;
            this.d2 = i2;
        }
    }

    public class Queries
    {
        public static IEnumerable<Data> all(List<Data> data)
        {
            return (from x in data select x).ToList();
        }

        public static IEnumerable<object> all_id_value(List<Data> data)
        {
            var q = from x in data
                    select new { IDENTIFIER = x.id, VALUE = x.value };
            return q;
        }
    }
}
