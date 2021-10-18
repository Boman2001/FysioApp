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
            optionsBuilder.UseSqlServer("server=host.docker.internal,1434;user=sa;password=8jkGh47hnDw89Haq8LN2;Database=FysioData;MultipleActiveResultSets=True;");

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}