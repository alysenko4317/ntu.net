using System;
using System.Collections.Generic;
using Npgsql;

namespace PgsqlAdoDemo
{
    // -------------------------------------------------------------------
    // Entities
    // -------------------------------------------------------------------

    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    // -------------------------------------------------------------------
    // DAO Layer
    // -------------------------------------------------------------------

    public interface IUserDao {
        List<User> GetAllUsers();
    }

    public class UserDao : IUserDao
    {
        private readonly string _connectionString;

        public UserDao(string connectionString) {
            _connectionString = connectionString;
        }

        public List<User> GetAllUsers()
        {
            var users = new List<User>();

            using (var conn = new NpgsqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT account_id, first_name, last_name FROM car_portal_app.account", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var user = new User {
                            Id = reader.GetInt32(0),
                            FirstName = reader.GetString(1),
                            LastName = reader.GetString(2)
                        };

                        users.Add(user);
                    }
                }
            }

            return users;
        }
    }

    // -------------------------------------------------------------------
    // Main
    // -------------------------------------------------------------------

    class Program
    {
        static void Main()
        {
            string userName = "postgres";
            string userPass = "1234";
            string connString = $"Host=localhost;Port=5432;Username={userName};Password={userPass};Database=car_portal";

            IUserDao userDao = new UserDao(connString);

            List<User> users = userDao.GetAllUsers();

            foreach (var user in users) {
                Console.WriteLine($"{user.Id} - {user.FirstName} {user.LastName}");
            }
        }
    }
}
