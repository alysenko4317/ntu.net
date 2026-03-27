using Microsoft.EntityFrameworkCore;
using laba_3_1.Models;
using System.Collections.Generic;

namespace laba_3_1.Models
{
    public class CompanyModelContainer : DbContext
    {
        public DbSet<Employee> Workers { get; set; }
        public DbSet<Department> Departments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                 @"Server=(localdb)\mssqllocaldb;Database=CompanyWEBDB1;Trusted_Connection=True;MultipleActiveResultSets=true");
        }
    }
}
