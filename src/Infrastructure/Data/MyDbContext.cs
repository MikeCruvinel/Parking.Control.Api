using Microsoft.EntityFrameworkCore;
using Parking.Control.Domain.Entities;

namespace Parking.Control.Infrastructure.Data;

public partial class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ParkingSpace> ParkingSpaces { get; set; }

    public virtual DbSet<Vehicle> Vehicles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ParkingSpace>(entity =>
        {
            entity.HasOne(d => d.Vehicle).WithMany(p => p.ParkingSpaces)
                .HasForeignKey(d => d.VehicleId)
                .HasConstraintName("FK_ParkingSpaces_Vehicles");
        });

        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.Property(e => e.LicensePlate)
                .IsRequired()
                .HasMaxLength(10)
                .IsFixedLength();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}