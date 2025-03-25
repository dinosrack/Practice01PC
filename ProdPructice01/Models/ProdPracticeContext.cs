using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ProdPructice01.Models;

public partial class ProdPracticeContext : DbContext
{
    public ProdPracticeContext()
    {
    }

    public ProdPracticeContext(DbContextOptions<ProdPracticeContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CitiesWithAmdRyzen7> CitiesWithAmdRyzen7s { get; set; }

    public virtual DbSet<Computer> Computers { get; set; }

    public virtual DbSet<HighestFrequencyInCalifornium> HighestFrequencyInCalifornia { get; set; }

    public virtual DbSet<Manufacturer> Manufacturers { get; set; }

    public virtual DbSet<MarketOffer> MarketOffers { get; set; }

    public virtual DbSet<NewestAmdRyzen7Model> NewestAmdRyzen7Models { get; set; }

    public virtual DbSet<Seller> Sellers { get; set; }

    public virtual DbSet<TopManufacturerByRevenue> TopManufacturerByRevenues { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost\\sqlexpress; Database=ProdPractice; Trusted_Connection=True; TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CitiesWithAmdRyzen7>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("CitiesWithAMD_Ryzen7");

            entity.Property(e => e.Location).HasMaxLength(100);
        });

        modelBuilder.Entity<Computer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Computer__3214EC07E573C069");

            entity.Property(e => e.Cpufrequency)
                .HasColumnType("decimal(4, 2)")
                .HasColumnName("CPUFrequency");
            entity.Property(e => e.Cputype)
                .HasMaxLength(50)
                .HasColumnName("CPUType");
            entity.Property(e => e.Hdd).HasColumnName("HDD");
            entity.Property(e => e.ModelName).HasMaxLength(100);
            entity.Property(e => e.Ram).HasColumnName("RAM");

            entity.HasOne(d => d.Manufacturer).WithMany(p => p.Computers)
                .HasForeignKey(d => d.ManufacturerId)
                .HasConstraintName("FK__Computers__Manuf__267ABA7A");
        });

        modelBuilder.Entity<HighestFrequencyInCalifornium>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("HighestFrequencyInCalifornia");

            entity.Property(e => e.Cpufrequency)
                .HasColumnType("decimal(4, 2)")
                .HasColumnName("CPUFrequency");
            entity.Property(e => e.Location).HasMaxLength(100);
            entity.Property(e => e.ModelName).HasMaxLength(100);
        });

        modelBuilder.Entity<Manufacturer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Manufact__3214EC07C94D960E");

            entity.Property(e => e.Location).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<MarketOffer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MarketOf__3214EC079870F929");

            entity.Property(e => e.BatchPrice).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Computer).WithMany(p => p.MarketOffers)
                .HasForeignKey(d => d.ComputerId)
                .HasConstraintName("FK__MarketOff__Compu__2B3F6F97");

            entity.HasOne(d => d.Seller).WithMany(p => p.MarketOffers)
                .HasForeignKey(d => d.SellerId)
                .HasConstraintName("FK__MarketOff__Selle__2C3393D0");
        });

        modelBuilder.Entity<NewestAmdRyzen7Model>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("NewestAMD_Ryzen7_Models");

            entity.Property(e => e.ManufacturerName).HasMaxLength(100);
            entity.Property(e => e.ModelName).HasMaxLength(100);
        });

        modelBuilder.Entity<Seller>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Sellers__3214EC074E22544E");

            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(20);
        });

        modelBuilder.Entity<TopManufacturerByRevenue>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("TopManufacturerByRevenue");

            entity.Property(e => e.ManufacturerName).HasMaxLength(100);
            entity.Property(e => e.TotalRevenue).HasColumnType("decimal(38, 2)");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
