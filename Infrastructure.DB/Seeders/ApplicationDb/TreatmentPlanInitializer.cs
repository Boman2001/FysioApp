using Core.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Infrastructure.Seeders.ApplicationDb
{
    public class TreatmentPlanInitializer : IEntityTypeConfiguration<TreatmentPlan>
    {
        public void Configure(EntityTypeBuilder<TreatmentPlan> builder)
        {
            builder.ToTable("TreatmentPlans");
        }
    }
}