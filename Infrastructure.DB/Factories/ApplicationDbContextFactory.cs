using Core.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Core.Infrastructure.Factories
{
    public class ApplicationContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer("server=sql.bogaers.org,1434;user=FysioUser;password=VNGE9DRkH5jDzpkv;Database=FysioData;MultipleActiveResultSets=True;");
            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}