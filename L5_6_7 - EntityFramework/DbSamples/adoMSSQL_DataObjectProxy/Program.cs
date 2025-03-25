using System.Data.SqlClient;
using System.Configuration;
using System.Data;

// install System.Data.SqlClient
// install System.Configuration.ConfigurationManager

namespace DbDemo
{
    public class CustomerProxy
    {
        public int Id { get; set; }
        public string Name { get; set; }

       
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            var customers = GetCustomers();

            foreach (var customer in customers) {
                Console.WriteLine(
                    "Ідентифікатор: {0}\tФИО: {1}", customer.Id, customer.Name);
            }

            Console.ReadLine();
        }

        private static List<CustomerProxy> GetCustomers()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;
            
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                IDbCommand command = new SqlCommand("SELECT * FROM t_customer");
                command.Connection = connection;

                connection.Open();

                IDataReader reader = command.ExecuteReader();
                List<CustomerProxy> customers = new List<CustomerProxy>();

                while (reader.Read())
                {
                    CustomerProxy customer = new CustomerProxy();
                    customer.Id = reader.GetInt32(0);
                    customer.Name = reader.GetString(1);

                    customers.Add(customer);
                }

                return customers;
            }
        }
    }
}
