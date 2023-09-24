
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// Add the necessary NuGet packages for EF Core and the PostgreSQL provider.
//    Set up a DbContext class that represents the session with the database.

namespace PgsqlEFDemo
{
    // -------------------------------------------------------------------
    // Entities
    // -------------------------------------------------------------------
    //
    // To adapt these models for use with Entity Framework Core (EF Core), we'll need to:
    //    Add the necessary navigation properties to support relationships between entities.
    //    Add annotations or Fluent API configurations to further define the relationships
    //       and properties' constraints.

    [Table("account", Schema="car_portal_app")]
    public class Account
    {
        public Account() {
            Cars = new HashSet<Car>();
        }

        [Key]
        [Column("account_id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("first_name")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("last_name")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(150)]
        [Column("email")]
        public string Email { get; set; }

        [Required]
        [Column("password")]
        public string PasswordHash { get; set; }

        // Navigation property
        public virtual ICollection<Car> Cars { get; set; }
    }

    [Table("car", Schema="car_portal_app")]
    public class Car
    {
        [Key]
        [Column("car_id")]
        public int Id { get; set; }

        // Navigation property for many-to-many relationship
        public virtual ICollection<Account> Owners { get; set; }

        // Foreign key for CarModel
        [Column("car_model_id")]
        public int CarModelId { get; set; }

        // Navigation property
        [ForeignKey("CarModelId")]
        public virtual CarModel Model { get; set; }

        [Required]
        [MaxLength(20)]
        [Column("registration_number")]
        public string RegistrationNumber { get; set; }

        [Range(1900, 2100)]
        [Column("manufacture_year")]
        public int ManufactureYear { get; set; }

        [Column("mileage")]
        public int Mileage { get; set; }
    }

    [Table("car_model", Schema="car_portal_app")]
    public class CarModel
    {
        [Key]
        [Column("car_model_id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("make")]
        public string Make { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("model")]
        public string Model { get; set; }
    }

    // -------------------------------------------------------------------
    // DB Context
    // -------------------------------------------------------------------

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // In this configuration, we're specifying a dictionary (Dictionary<string, object>)
            // as the type for the join entity since you don't have an explicit class for the
            // car_account_link table.The HasOne and HasForeignKey methods define the relationships
            // and the foreign key columns on the join table.

            modelBuilder.Entity<Account>()
                .HasMany(a => a.Cars)
                .WithMany(c => c.Owners)
                .UsingEntity<Dictionary<string, object>>(
                    "car_account_link",
                    j => j.HasOne<Car>().WithMany().HasForeignKey("car_id"),
                    j => j.HasOne<Account>().WithMany().HasForeignKey("account_id")
                );
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<CarModel> CarModels { get; set; }
    }

    // -------------------------------------------------------------------
    // DAO Layer
    // -------------------------------------------------------------------

    // To adapt your AccountRepository for use with Entity Framework Core, you'll need to make several changes.
    //    Instead of using a raw connection string, we'll leverage the DbContext provided by EF Core.
    //    Use the EF Core's LINQ methods to query and manipulate data.

    public interface IAccountDAO
    {
        List<Account> FetchAll();
        List<Account> FindAllByFirstName(string firstName);
        Account Find(int id);
        void Save(Account model);
        void Update(Account model);
        void Delete(int id);
    }

    public class AccountRepository : IAccountDAO
    {
        private readonly AppDbContext _context;
        private readonly bool _loadOwnedCars;

        // if default value for P2 is removed will get:
        //    var accountRepo = serviceProvider.GetRequiredService<IAccountDAO>();
        //       System.InvalidOperationException: 'Unable to resolve service for type 'System.Boolean'
        //       while attempting to activate 'PgsqlEFDemo.AccountRepository'.'
        // that means when trying to create an instance of AccountRepository, the dependency injection
        // container is unable to provide a value for a boolean parameter 2.
        //
        // In the configuration we've provided with ServiceCollection, only the IAccountDAO
        // to AccountRepository mapping is provided. However, there's no instruction on how
        // to satisfy the bool loadOwnedCars parameter.
        //
        // To resolve this, you can:
        //
        // Modify the constructor of AccountRepository to only require the dependencies that
        // can be resolved by the DI container.
        //
        // Provide a default value for the bool parameter or remove it if it's not required.
        //
        // If you need to keep the boolean parameter, you can register it as a singleton in the DI
        // container so that it can be resolved when required.
        //
        public AccountRepository(AppDbContext context, bool loadOwnedCars = true)
        {
            _context = context;
            _loadOwnedCars = loadOwnedCars;
        }

        public List<Account> FetchAll()
        {
            var query = _context.Accounts.AsQueryable();

            if (_loadOwnedCars)
            {
                query = query
                    .Include(a => a.Cars)
                    .ThenInclude(c => c.Model);
            }

            return query.OrderBy(a => a.FirstName).ToList();
        }

        public Account Find(int id)
        {
            if (_loadOwnedCars)
            {
                return _context.Accounts
                               .Include(a => a.Cars)
                               .ThenInclude(c => c.Model)
                               .FirstOrDefault(a => a.Id == id);
            }
            else
            {
                return _context.Accounts.Find(id);
            }
        }

        public List<Account> FindAllByFirstName(string firstName)
        {
            var query = _context.Accounts.AsQueryable();

            if (_loadOwnedCars)
            {
                query = query.Include(a => a.Cars)
                             .ThenInclude(c => c.Model);
            }

            return query.Where(a => a.FirstName == firstName).ToList();
        }

        public void Save(Account model)
        {
            _context.Accounts.Add(model);
            _context.SaveChanges();
        }

        public void Update(Account model)
        {
            _context.Entry(model).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var account = _context.Accounts.Find(id);
            if (account != null)
            {
                _context.Accounts.Remove(account);
                _context.SaveChanges();
            }
        }
    }

    // -------------------------------------------------------------------
    // Main
    // -------------------------------------------------------------------

    class Program
    {
        static void Main()
        {
            // Setup Dependency Injection
            var services = new ServiceCollection()
                .AddDbContext<AppDbContext>(options =>
                    options.UseNpgsql(
                        "Host=localhost;Port=5433;Username=postgres;Password=1234;Database=car_portal")
                    )
                .AddTransient<IAccountDAO, AccountRepository>();
                //.BuildServiceProvider();

            var serviceProvider = services.BuildServiceProvider();

            // List all the registered services
            foreach (var serviceDescriptor in services) {
                Console.WriteLine($"Service: {serviceDescriptor.ServiceType.FullName}");
                if (serviceDescriptor.ImplementationType != null)
                    Console.WriteLine($"\tImplementation: {serviceDescriptor.ImplementationType.FullName}");
            }

            var accountRepo = serviceProvider.GetRequiredService<IAccountDAO>();

            List<Account> accounts = accountRepo.FetchAll();

            // Uncomment to print all users
            /*
            foreach (var account in accounts)  {
                Console.WriteLine($"{account.Id} - {account.FirstName} {account.LastName}");
            }
            */

            // print only users that own cars
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
