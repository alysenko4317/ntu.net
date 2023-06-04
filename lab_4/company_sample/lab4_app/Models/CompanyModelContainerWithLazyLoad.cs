using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace laba_3_1.Models
{

    public class CompanyModelContainerWithLazyLoad : CompanyModelContainer
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseLazyLoadingProxies();
            base.OnConfiguring(optionsBuilder);
        }
    }

}
