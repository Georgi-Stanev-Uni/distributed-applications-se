using Microsoft.EntityFrameworkCore;
using CarServiceMVC.Models;

namespace CarServiceMVC.Data
{
    public class CarServiceDbContext : DbContext
    {
        public CarServiceDbContext(DbContextOptions<CarServiceDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=DESKTOP-2T34TGN;Database=CarServiceDB;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true;");
            }
        }

        public DbSet<CustomerModel> Customers { get; set; }
        public DbSet<MechanicModel> Mechanics { get; set; }
        public DbSet<CarModel> Cars { get; set; }
        public DbSet<RepairModel> Repairs { get; set; }
        public DbSet<CustomerMechanicModel> CustomerMechanics { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Composite Key: CustomerMechanic
            modelBuilder.Entity<CustomerMechanicModel>()
                .HasKey(cm => new { cm.CustomerID, cm.MechanicID });

            // One-to-Many: Customer -> Cars
            modelBuilder.Entity<CustomerModel>()
                .HasMany(c => c.Cars)
                .WithOne(car => car.Customer)
                .HasForeignKey(car => car.CustomerID)
                .OnDelete(DeleteBehavior.Restrict);

            // One-to-Many: Car -> Repairs
            modelBuilder.Entity<CarModel>()
                .HasMany(c => c.Repairs)
                .WithOne(r => r.Car)
                .HasForeignKey(r => r.CarID)
                .OnDelete(DeleteBehavior.Restrict);

            // One-to-Many: Mechanic -> Repairs
            modelBuilder.Entity<MechanicModel>()
                .HasMany(m => m.Repairs)
                .WithOne(r => r.Mechanic)
                .HasForeignKey(r => r.MechanicID)
                .OnDelete(DeleteBehavior.Restrict);

            // Many-to-Many: Customer <-> Mechanic
            modelBuilder.Entity<CustomerMechanicModel>()
                .HasOne(cm => cm.Customer)
                .WithMany(c => c.CustomerMechanics)
                .HasForeignKey(cm => cm.CustomerID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CustomerMechanicModel>()
                .HasOne(cm => cm.Mechanic)
                .WithMany(m => m.CustomerMechanics)
                .HasForeignKey(cm => cm.MechanicID)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}
