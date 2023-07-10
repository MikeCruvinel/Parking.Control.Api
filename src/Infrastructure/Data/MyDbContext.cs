using Microsoft.EntityFrameworkCore;
using Parking.Control.Domain.Entities;

namespace Parking.Control.Infrastructure.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }

        public DbSet<ParkingEntity> Parking { get; set; }
        public DbSet<ParkingSpacesCount> ParkingSpacesCount { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ParkingEntity>().ToTable("Parking");
            modelBuilder.Entity<ParkingSpacesCount>().ToTable("ParkingSpaces").HasNoKey() ;
        }
    }
}
