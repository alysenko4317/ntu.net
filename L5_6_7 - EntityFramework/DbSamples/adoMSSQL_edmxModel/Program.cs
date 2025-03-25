using ConsoleApp1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Program
    {
        private static List<Customer> GetCustomersEf()
        {
            var context = new testDBEntities();
            var customers = context.Customers.ToList();
            return customers;
        }

        private static void Main(string[] args)
        {
            var customers = GetCustomersEf();

            foreach (var customer in customers)
            {
                Console.WriteLine("Идентификатор: {0}\tИмя: {1}",
                    customer.CustomerId, customer.CustomerName);
            }

            Console.ReadLine();
        }
    }
}
