using ConsoleApp1;
using ConsoleApp2.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Scaffold-DbContext "Server=(localdb)\MSSQLLocalDB;Database=testDB;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models
namespace ConsoleApp1
{
    internal class Program
    {
        private static List<TCustomer> GetCustomers()
        {
            var context = new TestDbContext();

            //IQueryable<TCustomer> query = context.TCustomers;
             IQueryable<TCustomer> query = context.TCustomers.Where(c => c.CustomerId == 1);

           //  IEnumerable<TCustomer> query =
           //      (context.TCustomers as IEnumerable<TCustomer>).Where(c => c.CustomerId == 1);

            // c - змінна, з якою ми будемо працювати
            var query = from c in context.TCustomers
                        where c.CustomerId == 1
                        select c;

            List<TCustomer> customers = query.ToList();

            return customers;
        }

        private static void test()
        {
            var context = new TestDbContext();

            var transactions = from c in context.TCustomers
                               join o in context.TOrders on c.CustomerId equals o.CustomerId
                               join op in context.TOrderProducts on o.OrderId equals op.OrderId
                               join p in context.TProducts on op.ProductId equals p.ProductId
                               select new
                               {
                                   CustomerName = c.CustomerName,
                                   ProductName = p.ProductName,
                                   ProductCount = op.Count
                               };

            foreach (var transaction in transactions)
            {
                Console.WriteLine("Покупатель: {0}\tПродукт: {1}\tКоличество: {2}",
                                  transaction.CustomerName, transaction.ProductName, transaction.ProductCount);
            }
        }
        
        private static void Main(string[] args)
        {
            var customers = GetCustomers();

            foreach (var customer in customers)
            {
                Console.WriteLine("Идентификатор: {0}\tИмя: {1}",
                    customer.CustomerId, customer.CustomerName);
            }

            test();

            Console.ReadLine();
        }
    }
}