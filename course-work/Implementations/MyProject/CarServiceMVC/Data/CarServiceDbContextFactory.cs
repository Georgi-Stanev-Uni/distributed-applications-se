using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CarServiceMVC.Data
{
    public class CarServiceDbContextFactory : IDesignTimeDbContextFactory<CarServiceDbContext>
    {
        public CarServiceDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CarServiceDbContext>();
            optionsBuilder.UseSqlServer(@"Server=DESKTOP-2T34TGN;Database=CarServiceDB;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true;");

            return new CarServiceDbContext(optionsBuilder.Options);
        }
    }
}
