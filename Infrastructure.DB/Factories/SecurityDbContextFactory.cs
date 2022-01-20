using Core.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Core.Infrastructure.Factories
{
    public class SecurityDbContextFactory : IDesignTimeDbContextFactory<SecurityDbContext>
    {
        public SecurityDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SecurityDbContext>();
            optionsBuilder.UseSqlServer("server=sql.bogaers.org,1434;user=FysioUser;password=VNGE9DRkH5jDzpkv;Database=FysioIdentity;MultipleActiveResultSets=True;");

            return new SecurityDbContext(optionsBuilder.Options);
        }
    }
}