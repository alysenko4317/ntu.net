using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EFDemo_DBFirstPG.Models;

public partial class CarPortalContext : DbContext
{
    public CarPortalContext()
    {
    }

    public CarPortalContext(DbContextOptions<CarPortalContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<AccountHistory> AccountHistories { get; set; }

    public virtual DbSet<Advertisement> Advertisements { get; set; }

    public virtual DbSet<AdvertisementPicture> AdvertisementPictures { get; set; }

    public virtual DbSet<AdvertisementRating> AdvertisementRatings { get; set; }

    public virtual DbSet<Car> Cars { get; set; }

    public virtual DbSet<CarAccountLink> CarAccountLinks { get; set; }

    public virtual DbSet<CarModel> CarModels { get; set; }

    public virtual DbSet<FavoriteAd> FavoriteAds { get; set; }

    public virtual DbSet<Foo> Foos { get; set; }

    public virtual DbSet<SellerAccount> SellerAccounts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5433;Username=postgres;Password=1234;Database=car_portal");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("en_US.UTF-8")
            .HasPostgresExtension("postgis")
            .HasPostgresExtension("topology", "postgis_topology");

        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("account_pkey");

            entity.ToTable("account", "car_portal_app");

            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(255)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(255)
                .HasColumnName("last_name");
            entity.Property(e => e.LoginDate).HasColumnName("login_date");
            entity.Property(e => e.OwnerAccountId).HasColumnName("_owner_account_id");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");

            entity.HasOne(d => d.OwnerAccount).WithMany(p => p.InverseOwnerAccount)
                .HasForeignKey(d => d.OwnerAccountId)
                .HasConstraintName("fksyfr3l0p6x8xi9vuyie6mrw0d");
        });

        modelBuilder.Entity<AccountHistory>(entity =>
        {
            entity.HasKey(e => e.AccountHistoryId).HasName("account_history_pkey");

            entity.ToTable("account_history", "car_portal_app");

            entity.HasIndex(e => new { e.AccountId, e.SearchKey, e.SearchDate }, "account_history_account_id_search_key_search_date_key").IsUnique();

            entity.Property(e => e.AccountHistoryId).HasColumnName("account_history_id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.SearchDate).HasColumnName("search_date");
            entity.Property(e => e.SearchKey).HasColumnName("search_key");
        });

        modelBuilder.Entity<Advertisement>(entity =>
        {
            entity.HasKey(e => e.AdvertisementId).HasName("advertisement_pkey");

            entity.ToTable("advertisement", "car_portal_app");

            entity.Property(e => e.AdvertisementId).HasColumnName("advertisement_id");
            entity.Property(e => e.AdvertisementDate).HasColumnName("advertisement_date");
            entity.Property(e => e.CarId).HasColumnName("car_id");
            entity.Property(e => e.SellerAccountId).HasColumnName("seller_account_id");

            entity.HasOne(d => d.Car).WithMany(p => p.Advertisements)
                .HasForeignKey(d => d.CarId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("advertisement_car_id_fkey");

            entity.HasOne(d => d.SellerAccount).WithMany(p => p.Advertisements)
                .HasForeignKey(d => d.SellerAccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("advertisement_seller_account_id_fkey");
        });

        modelBuilder.Entity<AdvertisementPicture>(entity =>
        {
            entity.HasKey(e => e.AdvertisementPictureId).HasName("advertisement_picture_pkey");

            entity.ToTable("advertisement_picture", "car_portal_app");

            entity.HasIndex(e => e.PictureLocation, "advertisement_picture_picture_location_key").IsUnique();

            entity.Property(e => e.AdvertisementPictureId).HasColumnName("advertisement_picture_id");
            entity.Property(e => e.AdvertisementId).HasColumnName("advertisement_id");
            entity.Property(e => e.PictureLocation).HasColumnName("picture_location");

            entity.HasOne(d => d.Advertisement).WithMany(p => p.AdvertisementPictures)
                .HasForeignKey(d => d.AdvertisementId)
                .HasConstraintName("advertisement_picture_advertisement_id_fkey");
        });

        modelBuilder.Entity<AdvertisementRating>(entity =>
        {
            entity.HasKey(e => e.AdvertisementRatingId).HasName("advertisement_rating_pkey");

            entity.ToTable("advertisement_rating", "car_portal_app");

            entity.Property(e => e.AdvertisementRatingId).HasColumnName("advertisement_rating_id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.AdvertisementId).HasColumnName("advertisement_id");
            entity.Property(e => e.AdvertisementRatingDate).HasColumnName("advertisement_rating_date");
            entity.Property(e => e.Rank).HasColumnName("rank");
            entity.Property(e => e.Review).HasColumnName("review");

            entity.HasOne(d => d.Advertisement).WithMany(p => p.AdvertisementRatings)
                .HasForeignKey(d => d.AdvertisementId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("advertisement_rating_advertisement_id_fkey");
        });

        modelBuilder.Entity<Car>(entity =>
        {
            entity.HasKey(e => e.CarId).HasName("car_pkey");

            entity.ToTable("car", "car_portal_app");

            entity.HasIndex(e => e.RegistrationNumber, "car_registration_number_key").IsUnique();

            entity.Property(e => e.CarId).HasColumnName("car_id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.CarModelId).HasColumnName("car_model_id");
            entity.Property(e => e.ManufactureYear).HasColumnName("manufacture_year");
            entity.Property(e => e.Manufactureyear1).HasColumnName("_manufactureyear");
            entity.Property(e => e.Mileage).HasColumnName("mileage");
            entity.Property(e => e.Mileage1).HasColumnName("_mileage");
            entity.Property(e => e.NumberOfDoors)
                .HasDefaultValueSql("5")
                .HasColumnName("number_of_doors");
            entity.Property(e => e.NumberOfOwners).HasColumnName("number_of_owners");
            entity.Property(e => e.RegistrationNumber).HasColumnName("registration_number");
            entity.Property(e => e.Registrationnumber1)
                .HasMaxLength(255)
                .HasColumnName("_registrationnumber");

            entity.HasOne(d => d.Account).WithMany(p => p.Cars)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("fk8yccywtga09wogkpiv7g72c14");

            entity.HasOne(d => d.CarModel).WithMany(p => p.Cars)
                .HasForeignKey(d => d.CarModelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("car_car_model_id_fkey");
        });

        modelBuilder.Entity<CarAccountLink>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("car_account_link", "car_portal_app");

            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.CarId).HasColumnName("car_id");

            entity.HasOne(d => d.Account).WithMany()
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk3mhn2tln4kmgpfuqoi7qf2qpw");

            entity.HasOne(d => d.Car).WithMany()
                .HasForeignKey(d => d.CarId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fkfjtwwbs5659h9lob4karv62sn");
        });

        modelBuilder.Entity<CarModel>(entity =>
        {
            entity.HasKey(e => e.CarModelId).HasName("car_model_pkey");

            entity.ToTable("car_model", "car_portal_app");

            entity.HasIndex(e => new { e.Make, e.Model }, "car_model_make_model_key").IsUnique();

            entity.Property(e => e.CarModelId)
                .ValueGeneratedOnAdd()
                .HasColumnName("car_model_id");
            entity.Property(e => e.CarModelAge).HasColumnName("car_model_age");
            entity.Property(e => e.Make).HasColumnName("make");
            entity.Property(e => e.Model).HasColumnName("model");
            entity.Property(e => e.ModelCarModelId).HasColumnName("_model_car_model_id");

            entity.HasOne(d => d.CarModelNavigation).WithOne(p => p.CarModelNavigation)
                .HasForeignKey<CarModel>(d => d.CarModelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk5ke4nawwfjb557dhxxwxjwr9d");

            entity.HasOne(d => d.ModelCarModel).WithMany(p => p.InverseModelCarModel)
                .HasForeignKey(d => d.ModelCarModelId)
                .HasConstraintName("fkhdmvt6ye3a3vas8tkym2aw81q");
        });

        modelBuilder.Entity<FavoriteAd>(entity =>
        {
            entity.HasKey(e => new { e.AccountId, e.AdvertisementId }).HasName("favorite_ads_pkey");

            entity.ToTable("favorite_ads", "car_portal_app");

            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.AdvertisementId).HasColumnName("advertisement_id");

            entity.HasOne(d => d.Advertisement).WithMany(p => p.FavoriteAds)
                .HasForeignKey(d => d.AdvertisementId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("favorite_ads_advertisement_id_fkey");
        });

        modelBuilder.Entity<Foo>(entity =>
        {
            entity.HasKey(e => e.Pk).HasName("foo_pkey");

            entity.ToTable("foo", "car_portal_app");

            entity.Property(e => e.Pk).HasColumnName("_pk");
            entity.Property(e => e.Bar1)
                .HasMaxLength(255)
                .HasColumnName("_bar1");
            entity.Property(e => e.Bar2).HasColumnName("_bar2");
        });

        modelBuilder.Entity<SellerAccount>(entity =>
        {
            entity.HasKey(e => e.SellerAccountId).HasName("seller_account_pkey");

            entity.ToTable("seller_account", "car_portal_app");

            entity.Property(e => e.SellerAccountId).HasColumnName("seller_account_id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.City).HasColumnName("city");
            entity.Property(e => e.NumberOfAdvertisement).HasColumnName("number_of_advertisement");
            entity.Property(e => e.StreetName).HasColumnName("street_name");
            entity.Property(e => e.StreetNumber).HasColumnName("street_number");
            entity.Property(e => e.TotalRank).HasColumnName("total_rank");
            entity.Property(e => e.ZipCode).HasColumnName("zip_code");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
