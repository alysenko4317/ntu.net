using System;
using Npgsql;

// INSTALL npgsql package before compile
//======================================
//
// Npgsql is an ADO.NET data provider for PostgreSQL.\
// ADO.NET is a set of classes in the .NET framework that provides data access services to relational databases,
// and Npgsql extends ADO.NET to work specifically with PostgreSQL.
//
// The ADO.NET model is centered around the concept of data providers:
//    sets of classes that can connect to a database, execute commands, and retrieve results.
// Each major database typically has its own ADO.NET data provider.
// System.Data.SqlClient is the data provider for SQL Server, while Npgsql is the data provider for PostgreSQL.
//
// With Npgsql, you can use familiar ADO.NET patterns and classes like DbConnection, DbCommand, and DbDataReader,
// but with PostgreSQL-specific implementations (NpgsqlConnection, NpgsqlCommand, NpgsqlDataReader, etc.).

namespace ADONetPostgresApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string userName = "postgres";
            string userPass = "1234";
            string connString = $"Host=localhost;Port=5432;Username={userName};Password={userPass};Database=car_portal";

            using (var connection = new NpgsqlConnection(connString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand("SELECT * FROM car_portal_app.account", connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine(reader.GetString(reader.GetOrdinal("first_name")) + " "
                                            + reader.GetString(reader.GetOrdinal("last_name")));
                    }
                }
            }
        }
    }
}
