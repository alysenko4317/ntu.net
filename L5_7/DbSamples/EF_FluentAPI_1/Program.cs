using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Logging;

// С помощью атрибутов и Fluent API для сущостей и их свойств можно установить многочисленные настройки.
// Однако, если настроек очень много, то они могут утяжелять класс контекста и сущностей. В этом случае
// Entity Framework Core позволяет вынести конфигурацию сущностей в отдельные классы.

public class ApplicationContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Company> Companies { get; set; } = null!;

    public ApplicationContext()
    {
        Database.EnsureDeleted();
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=helloapp.db");

        // Configure logging to capture and print SQL queries
        //
        // While direct calls to .LogTo(Console.WriteLine, LogLevel.Information) can be useful
        // for quick debugging or logging in small applications, they lack the configurability,
        // scalability, and maintainability that dedicated logging packages offer.
        // As your application grows and matures, using a logging package becomes a more robust
        // and sustainable choice.

        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        optionsBuilder.UseLoggerFactory(loggerFactory);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Теперь конфигурация моделей вынесена в отдельные классы. А для добавления конкретных
        // конфигураций в контекст используется метод modelBuilder.ApplyConfiguration(), которому
        // передается нужный объект конфигурации

        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new CompanyConfiguration());
    }
}

//
// Для вынесения конфигурации во вне необходимо создать класс конфигурации, реализующий интерфейс
// EntityTypeConfiguration<T>.
//
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("People").Property(p => p.Name).IsRequired();
        builder.Property(p => p.Id).HasColumnName("user_id");
    }
}

public class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.ToTable("Enterprises").Property(c => c.Name).IsRequired();
    }
}

public class User
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int Age { get; set; }
}

public class Company
{
    public int Id { get; set; }
    public string? Name { get; set; }
}

public class Program
{
    public static void Main()
    {
        using (var context = new ApplicationContext())
        {
            // Example usage of the DbContext
            var user = new User { Name = "John", Age = 30 };
            context.Users.Add(user);
            context.SaveChanges();
        }
    }
}
