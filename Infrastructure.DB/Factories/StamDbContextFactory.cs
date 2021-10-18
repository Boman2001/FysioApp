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
            optionsBuilder.UseSqlServer("server=host.docker.internal,1434;user=sa;password=8jkGh47hnDw89Haq8LN2;Database=FysioStam;MultipleActiveResultSets=True;");

            return new StamDbContext(optionsBuilder.Options);
        }
    }
}