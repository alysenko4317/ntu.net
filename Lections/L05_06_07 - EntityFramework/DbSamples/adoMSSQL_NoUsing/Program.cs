using System;
using System.Data;
using System.Data.SqlClient;

// install System.Data.SqlClient

namespace ADONetConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string connString =
                @"Server=(localdb)\MSSQLLocalDB;Database=testDB;Trusted_Connection=True;";

            SqlConnection connection = null;
            SqlCommand command = null;
            SqlDataReader reader = null;

            try
            {
                connection = new SqlConnection(connString);
                connection.Open();

                command = new SqlCommand("SELECT * FROM t_customer", connection);
                reader = command.ExecuteReader();

                Console.WriteLine("Id | SubjectName");
                Console.WriteLine("-----------------");

                while (reader.Read())
                {
                    int userId = reader.GetInt32(0);
                    string userName = reader.GetString(1);

                    Console.WriteLine($"{userId} | {userName}");
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may arise here
                Console.WriteLine("An error occurred: " + ex.Message);
            }
            finally
            {
                // Ensure that all resources are closed and disposed of
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                }

                if (command != null)
                {
                    command.Dispose();
                }

                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }
    }
}




