using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Core.Infrastructure.Contexts
{
    public class SecurityDbContext : IdentityDbContext
    {
        public SecurityDbContext(DbContextOptions<SecurityDbContext> contextOptions) : base(contextOptions)
        {
        }
        
    }
}