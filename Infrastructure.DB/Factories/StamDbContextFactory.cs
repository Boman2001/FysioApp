using Core.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Core.Infrastructure.Factories
{
    public class StamDbContextFactory : IDesignTimeDbContextFactory<StamDbContext>
    {
        public StamDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<StamDbContext>();
            optionsBuilder.UseSqlServer("server=sql.bogaers.org,1434;user=FysioUser;password=VNGE9DRkH5jDzpkv;Database=FysioStam;MultipleActiveResultSets=True;");

            return new StamDbContext(optionsBuilder.Options);
        }
    }
}