﻿using System;
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

            using (SqlConnection connection = new SqlConnection(connString))
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
