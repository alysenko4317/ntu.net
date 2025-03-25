using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApp2.Models;

public partial class TestDbContext : DbContext
{
    public TestDbContext() {
    }

    public TestDbContext(DbContextOptions<TestDbContext> options)
        : base(options) {
    }

    public virtual DbSet<TCustomer> TCustomers { get; set; }
    public virtual DbSet<TOrder> TOrders { get; set; }
    public virtual DbSet<TOrderProduct> TOrderProducts { get; set; }
    public virtual DbSet<TProduct> TProducts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            "Server=(localdb)\\MSSQLLocalDB;Database=testDB;Trusted_Connection=True;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TCustomer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__t_custom__A4AE64D8DD8EBFD9");
            entity.ToTable("t_customer");
            entity.Property(e => e.CustomerId).ValueGeneratedNever();
            entity.Property(e => e.CustomerName).HasMaxLength(100);
        });

        modelBuilder.Entity<TOrder>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__t_order__C3905BCFA0A3F22C");
            entity.ToTable("t_order");
            entity.Property(e => e.OrderId).ValueGeneratedNever();
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.HasOne(d => d.Customer).WithMany(p => p.TOrders)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_Order_Customer");
        });

        modelBuilder.Entity<TOrderProduct>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.ProductId }).HasName("PK__t_order___08D097A3B5795BB6");

            entity.ToTable("t_order_product");

            entity.HasOne(d => d.Order).WithMany(p => p.TOrderProducts)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderProduct_Order");

            entity.HasOne(d => d.Product).WithMany(p => p.TOrderProducts)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderProduct_Product");
        });

        modelBuilder.Entity<TProduct>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__t_produc__B40CC6CDB045C750");
            entity.ToTable("t_product");
            entity.Property(e => e.ProductId).ValueGeneratedNever();
            entity.Property(e => e.ProductName).HasMaxLength(100);
            entity.Property(e => e.ProductPrice).HasColumnType("decimal(10, 2)");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
