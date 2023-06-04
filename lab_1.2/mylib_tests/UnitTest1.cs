
using Xunit;
using SimpleLINQ;
using System.Collections.Generic;

namespace TestProject1
{
    public class UnitTest1
    {
        static List<Data> d1 = new List<Data>()
        {
            new Data(1, "group1", "11"),
            new Data(2, "group1", "12"),
            new Data(3, "group2", "13"),
            new Data(5, "group2", "15")
        };

        public readonly string[] expected_all =
        {
            "(id=1; grp=group1; value=11)",
            "(id=2; grp=group1; value=12)",
            "(id=3; grp=group2; value=13)",
            "(id=5; grp=group2; value=15)"
        };

        public readonly string[] expected_all_id_value =
        {
            "{ IDENTIFIER = 1, VALUE = 11 }",
            "{ IDENTIFIER = 2, VALUE = 12 }",
            "{ IDENTIFIER = 3, VALUE = 13 }",
            "{ IDENTIFIER = 5, VALUE = 15 }"
        };

        [Fact]
        public void Test1()
        {
            var q = Queries.all_id_value(d1);

            int i = 0;
            foreach (var x in q)
                Assert.Equal(expected_all_id_value[i++], x.ToString());
        }

        [Fact]
        public void Test2()
        {
            var q = Queries.all(d1);

            int i = 0;
            foreach (var x in q)
                Assert.Equal(expected_all[i++], x.ToString());
        }
    }
}
