using laba_3_1.Models;

namespace laba_3_1.Models
{
    public class Queries
    {
        public static int SearchByName(string name)
        {
            CompanyModelContainer db = new CompanyModelContainerWithLazyLoad();
            var q1 = from x in db.Workers
                     where x.Name.Contains(name)
                     select x;
            int res = q1.Count();
            return res;
        }
    }
}
