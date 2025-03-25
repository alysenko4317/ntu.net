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

        private static void howto_LeftJoin()
        {
            var context = new TestDbContext();

            // LEFT JOIN impl
            // скільки всього продуктів купив кожний покупец
            var transactions = from c in context.TCustomers
                               join o in context.TOrders on c.CustomerId equals o.CustomerId into go
                               from oJoined in go.DefaultIfEmpty()

                               join op in context.TOrderProducts on oJoined.OrderId equals op.OrderId into gop
                               from opJoined in gop.DefaultIfEmpty()

                               join p in context.TProducts on opJoined.ProductId equals p.ProductId into gp
                               from pJoined in gp.DefaultIfEmpty()

                               select new
                               {
                                   CustomerName = c.CustomerName,
                                   ProductName = pJoined.ProductName,
                                   ProductCount = opJoined.Count ?? 0
                               };

            foreach (var transaction in transactions)
            {
                Console.WriteLine("Покупатель: {0}\tПродукт: {1}\tКоличество: {2}",
                                  transaction.CustomerName, transaction.ProductName, transaction.ProductCount);
            }

            Console.WriteLine(transactions.ToQueryString());
        }

        private static void howto_GroupBy()
        {
            var context = new TestDbContext();

            // GROUP BY impl
            // на яку загальну суму скупився кожний покупец
            var transactions = from c in context.TCustomers
                               join o in context.TOrders on c.CustomerId equals o.CustomerId into go
                               from oJoined in go.DefaultIfEmpty()

                               join op in context.TOrderProducts on oJoined.OrderId equals op.OrderId into gop
                               from opJoined in gop.DefaultIfEmpty()

                               join p in context.TProducts on opJoined.ProductId equals p.ProductId into gp
                               from pJoined in gp.DefaultIfEmpty()

                                // groupby складається з двох частин
                                // ліва частина new { c, opJoined, pJoined }
                                //    об'єднує в собі ті таблиці які так чи інакше використовуються в групуванні'
                                // права частина { c.CustomerId, c.CustomerName }
                                //    візначає атрибути по яким буде відбуватись групування
                               group new { c, opJoined, pJoined } by new { c.CustomerId, c.CustomerName } into g

                               select new
                               {
                                   // Ми не можемо використати c.CustomerName замість g.Key.CustomerName тут після операції
                                   // групування через те, що після виконання групування за допомогою операції group by ви
                                   // працюєте вже не з окремими записами(клієнтами), а з групами записів. Після групування
                                   // змінюється структура даних, і для доступу до полів, за якими ви групували, потрібно
                                   // використовувати ключ групи g.Key.
                                   //
                                   // Після виконання операції group by, кожен елемент у результаті представляє групу записів, де:
                                   //    g.Key — це ключ групи,
                                   // за яким відбулося групування(у вашому випадку, це CustomerId та CustomerName).
                                   //    g — це колекція всіх записів,
                                   // які потрапили до цієї групи.
                                   //
                                   // якщо не вказувати c.CustomerName у ключах групування (по факту для СУБД це наблишково оскільки
                                   // вказано c.CustomerId, а всі інші атрибути від нього функціонально залежать) доступ до ім'я користувача
                                   // можливо було б отримати через конструкцію
                                   //    CustomerName = g.First().CustomerName
                                   // але це меньш зручно

                                   CustomerName = g.Key.CustomerName,
                                   CustomerSum = g.Sum(t =>
                                       (t.opJoined == null ? 0 : t.opJoined.Count) *
                                       (t.pJoined == null ? 0 : t.pJoined.ProductPrice))
                               };

            var list = transactions.ToList();

            foreach (var c in list)
            {
                Console.WriteLine("Покупець: {0}\tСума: {1}",
                                  c.CustomerName, c.CustomerSum);
            }
        }

        private static void Main(string[] args)
        {
            //howto_LeftJoin();
            howto_GroupBy();

            Console.ReadLine();
        }
    }
}