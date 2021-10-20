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
            optionsBuilder.UseSqlServer("server=host.docker.internal,1434;user=sa;password=8jkGh47hnDw89Haq8LN2;Database=FysioIdentity;MultipleActiveResultSets=True;");

            return new SecurityDbContext(optionsBuilder.Options);
        }
    }
}