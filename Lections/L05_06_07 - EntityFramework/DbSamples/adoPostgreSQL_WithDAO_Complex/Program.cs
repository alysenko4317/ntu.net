using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Channels;
using Npgsql;

namespace PgsqlAdoDemo
{
    // -------------------------------------------------------------------
    // Entities
    // -------------------------------------------------------------------

    public class Account
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public List<Car> Cars { get; set; }
    }

    public class Car
    {
        public int Id { get; set; }
        public Account Owner { get; set; }
        public CarModel Model { get; set; }
        public string RegistrationNumber { get; set; }
        public int ManufactureYear { get; set; }
        public int Mileage { get; set; }
    }

    public class CarModel
    {
        public int Id { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
    }

    // -------------------------------------------------------------------
    // DAO Layer
    // -------------------------------------------------------------------

    public interface IAccountDAO
    {
        List<Account> FetchAll();
        List<Account> FindAllByFirstName(string firstName);
        Account Find(int id);
        void Save(Account model);
        void Update(Account model);
        void Delete(int id);
    }

    public class AccountMapper
    {
        private Dictionary<int, Account> _accounts = new Dictionary<int, Account>();
        private bool _loadOwnedCars;

        public AccountMapper(bool loadOwnedCars) {
            _loadOwnedCars = loadOwnedCars;
        }

        public Account MapToAccount(IDataRecord record)
        {
            int id = (int)record["account_id"];
            bool exists = false;// _accounts.ContainsKey(id);

            if (!exists)
            {
                List<Car> cars = null;

                if ((_loadOwnedCars && !record.IsDBNull(record.GetOrdinal("car_id")) && record.GetInt32(record.GetOrdinal("car_id")) != 0)
                    || (!_loadOwnedCars && !record.IsDBNull(record.GetOrdinal("cars_owned_count")) && record.GetInt32(record.GetOrdinal("cars_owned_count")) > 0))
                {
                    cars = new List<Car>();
                }

                _accounts[id] = new Account {
                    Id = id,
                    FirstName = (string)record["first_name"],
                    LastName = (string)record["last_name"],
                    Email = (string)record["email"],
                    PasswordHash = (string)record["password"],
                    Cars = cars
                };
            }

            Account account = _accounts[id];

            if (account.Cars != null && _loadOwnedCars)
            {
                Car car = new Car
                {
                    Id = (int) record["car_id"],
                    Owner = account,
                    Model = new CarModel {
                        Id = (int)record["car_model_id"],
                        Make = (string)record["make"],
                        Model = (string)record["model"]
                    },
                    RegistrationNumber = (string)record["registration_number"],
                    ManufactureYear = (int)record["manufacture_year"],
                    Mileage = (int)record["mileage"]
                };

                account.Cars.Add(car);
            }

            return exists ? null : account;
        }
    }

    public class AccountRepository : IAccountDAO
    {
        private readonly string _connectionString;
        private readonly bool _loadOwnedCars;
        private readonly AccountMapper _accountMapper;

        public AccountRepository(string connectionString, bool loadOwnedCars)
        {
            _connectionString = connectionString;
            _loadOwnedCars = loadOwnedCars;
            _accountMapper = new AccountMapper(loadOwnedCars);
        }

        public List<Account> FetchAll()
        {
            string query = _loadOwnedCars ? SQL_SELECT_ALL_WITH_CARS : SQL_SELECT_ALL;

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = new NpgsqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        List<Account> accounts = new List<Account>();
                        while (reader.Read())
                        {
                            Account account = _accountMapper.MapToAccount(reader);
                            if (account != null) // Filtering out null values 
                            {
                                accounts.Add(account);
                            }
                        }
                        return accounts;
                    }
                }
            }
        }

        public List<Account> FindAllByFirstName(string firstName) {
            throw new NotImplementedException();
        }

        public Account Find(int id) {
            throw new NotImplementedException();
        }

        public void Save(Account model) {
            throw new NotImplementedException();
        }

        public void Update(Account model) {
            throw new NotImplementedException();
        }

        public void Delete(int id) {
            throw new NotImplementedException();
        }

        // language=SQL
        private const string SQL_SELECT_ALL =
        "SELECT a.*, COUNT(b.account_id) AS cars_owned_count FROM car_portal_app.account a " +
            "LEFT JOIN car_portal_app.car_account_link b ON a.account_id = b.account_id " +
            "GROUP BY a.account_id, a.first_name " +
            "ORDER BY a.first_name";

        // language=SQL
        private const string SQL_SELECT_ALL_WITH_CARS =
            "SELECT a.*, c.*, m.* FROM car_portal_app.account a " +
            "LEFT JOIN car_portal_app.car_account_link b ON a.account_id = b.account_id " +
            "LEFT JOIN car_portal_app.car c ON b.car_id = c.car_id " +
            "LEFT JOIN car_portal_app.car_model m ON c.car_model_id = m.car_model_id " +
            "ORDER BY a.first_name";

        /*
        // language=SQL
        private const string SQL_SELECT_BY_ID =
            "SELECT * FROM car_portal_app.account WHERE account_id = @accountId";

        // language=SQL
        private const string SQL_SELECT_ALL_BY_FIRST_NAME =
            "SELECT * FROM car_portal_app.account WHERE first_name = @firstName";

        // language=SQL
        private const string SQL_SELECT_ALL_BY_FIRST_NAME_2 =
            "SELECT a.*, COUNT(b.account_id) AS cars_owned_count FROM car_portal_app.account a " +
                "LEFT JOIN car_portal_app.car_account_link b ON a.account_id = b.account_id " +
                "WHERE a.first_name = @firstName " +
                "GROUP BY a.account_id, a.first_name " +
                "ORDER BY a.first_name";

        // language=SQL
        private const string SQL_SELECT_ALL_BY_FIRST_NAME_WITH_CARS =
            "SELECT a.*, c.*, m.* FROM car_portal_app.account a " +
                "LEFT JOIN car_portal_app.car_account_link b ON a.account_id = b.account_id " +
                "LEFT JOIN car_portal_app.car c ON b.car_id = c.car_id " +
                "LEFT JOIN car_portal_app.car_model m ON c.car_model_id = m.car_model_id " +
                "WHERE a.first_name = @firstName " +
                "ORDER BY a.first_name";*/
    }

    // -------------------------------------------------------------------

    class Program
    {
        static void Main()
        {
            string userName = "postgres";
            string userPass = "1234";
            string connString = $"Host=localhost;Port=5432;Username={userName};Password={userPass};Database=car_portal";

            var accountRepo = new AccountRepository(connString, true);  // set true if you want to load cars with accounts

            List<Account> accounts = accountRepo.FetchAll();

            // print all users
            //foreach (var account in accounts) {
            //    Console.WriteLine($"{account.Id} - {account.FirstName} {account.LastName}");
            //}

            // print only users that owns cars
            foreach (var account in accounts)
            {
                if (account.Cars != null && account.Cars.Count > 0)
                {
                    Console.WriteLine($"{account.Id} - {account.FirstName} {account.LastName}");

                    foreach (var car in account.Cars)
                    {
                        Console.WriteLine(
                            $"\tCar ID: {car.Id}, Model: {car.Model.Make} {car.Model.Model}," +
                            $" Registration Number: {car.RegistrationNumber}," +
                            $" Manufacture Year: {car.ManufactureYear}," +
                            $" Mileage: {car.Mileage}");
                    }
                }
            }
        }
    }
}
