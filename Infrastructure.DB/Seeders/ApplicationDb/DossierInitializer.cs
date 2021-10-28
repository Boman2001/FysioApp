using Core.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Infrastructure.Seeders.ApplicationDb
{
    public class DossierInitializer : IEntityTypeConfiguration<Dossier>
    {
        public void Configure(EntityTypeBuilder<Dossier> builder)
        {
            builder.ToTable("Dossiers");
            
            builder
                .HasOne( dossier => dossier.Patient)
                .WithMany(patient => patient.Dossiers)
                .OnDelete(DeleteBehavior.NoAction);
            
            builder
                .HasOne( dossier => dossier.IntakeBy)
                .WithMany(user => user.IntakesDone)
                .OnDelete(DeleteBehavior.NoAction);
            
            builder
                .HasOne( dossier => dossier.SupervisedBy)
                .WithMany(user => user.IntakesSupervised)
                .OnDelete(DeleteBehavior.NoAction); 
            
            builder
                .HasMany<Appointment>( T => T.Appointments)
                .WithOne(T=> T.Dossier)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(dossier => dossier.TreatmentPlan);
        }
    }
}