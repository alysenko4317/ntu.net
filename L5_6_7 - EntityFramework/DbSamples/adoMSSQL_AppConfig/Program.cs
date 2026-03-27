using System.Data.SqlClient;
using System.Configuration;

// install System.Data.SqlClient
// install System.Configuration.ConfigurationManager

namespace ADONetConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;

            Console.WriteLine(connectionString);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT * FROM t_customer", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        Console.WriteLine("Id | SubjectName");
                        Console.WriteLine("-----------------");

                        while (reader.Read())
                        {
                            int userId = reader.GetInt32(0);
                            string userName = reader.GetString(1);

                            Console.WriteLine($"{userId} | {userName}");
                        }
                    }
                }
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
