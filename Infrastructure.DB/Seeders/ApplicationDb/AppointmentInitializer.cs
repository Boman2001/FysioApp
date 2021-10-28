using Core.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Infrastructure.Seeders.ApplicationDb
{
    public class AppointmentInitializer: IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            builder.HasDiscriminator<bool>("isTreatment")
                .HasValue<Appointment>(false).HasValue<Treatment>(true);
            builder
                .HasOne( treatment => treatment.ExcecutedBy)
                .WithMany(staff => staff.TreatmentsDone)
                .OnDelete(DeleteBehavior.Cascade);
            
        }
    }
}