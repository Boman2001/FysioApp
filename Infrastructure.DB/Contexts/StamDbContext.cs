using Core.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Core.Infrastructure.Contexts
{
    public class StamDbContext : DbContext
    {
        public StamDbContext()
        {
        }

        public StamDbContext(DbContextOptions<StamDbContext> contextOptions)
            : base(contextOptions)
        {
        }

        public DbSet<DiagnoseCode> DiagnoseCodes { get; set; }
        public DbSet<TreatmentCode> TreatmentsCodes { get; set; }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<DiagnoseCode>().ToTable("DiagnoseCodes");
            builder.Entity<TreatmentCode>().ToTable("TreatmentCodes");
        }
    }
}