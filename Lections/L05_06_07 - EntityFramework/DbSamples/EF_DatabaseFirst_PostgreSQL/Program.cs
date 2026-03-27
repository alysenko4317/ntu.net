using EFDemo_DBFirstPG.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

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
    private readonly CarPortalContext _context;
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
    public AccountRepository(CarPortalContext context, bool loadOwnedCars = true)
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
                .ThenInclude(c => c.CarModel);
        }

        return query.OrderBy(a => a.FirstName).ToList();
    }

    public Account Find(int id) {
        throw new NotImplementedException();
    }

    public List<Account> FindAllByFirstName(string firstName) {
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
}

class Program
{
    static void Main()
    {
        // ---------------------------------------------------
        // Setup Dependency Injection
        // ---------------------------------------------------

        var services = new ServiceCollection()
            .AddDbContext<CarPortalContext>(options =>
                options.UseNpgsql(
                    "Host=localhost;Port=5433;Username=postgres;Password=1234;Database=car_portal")
                )
            .AddTransient<IAccountDAO, AccountRepository>();

        var serviceProvider = services.BuildServiceProvider();

        // ---------------------------------------------------
        // List all the registered services
        // ---------------------------------------------------

        foreach (var serviceDescriptor in services)
        {
            Console.WriteLine($"Service: {serviceDescriptor.ServiceType.FullName}");
            if (serviceDescriptor.ImplementationType != null)
                Console.WriteLine($"\tImplementation: {serviceDescriptor.ImplementationType.FullName}");
            Console.WriteLine("");
        }

        // ---------------------------------------------------
        // Use previously registered service witch implements IAccountDAO interface
        // ---------------------------------------------------

        var accountRepo = serviceProvider.GetRequiredService<IAccountDAO>();

        List<Account> accounts = accountRepo.FetchAll();

        foreach (var account in accounts)  {
            Console.WriteLine($"{account.AccountId} - {account.FirstName} {account.LastName}");
        }
    }
}
