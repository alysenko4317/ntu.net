using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Logging;

// За допомогою атрибутів і Fluent API для сутностей та їхніх властивостей можна встановити численні налаштування.
// Однак, якщо налаштувань дуже багато, вони можуть ускладнювати клас контексту та сутностей.
// У цьому випадку Entity Framework Core дозволяє винести конфігурацію сутностей в окремі класи.

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
        // Тепер конфігурацію моделей винесено в окремі класи.
        // Для додавання конкретних конфігурацій у контекст використовується
        // метод modelBuilder.ApplyConfiguration(), якому передається потрібний об'єкт конфігурації.

        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new CompanyConfiguration());
    }
}

//
// Щоб винести конфігурацію назовні, потрібно створити клас конфігурації, який реалізує інтерфейс
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
