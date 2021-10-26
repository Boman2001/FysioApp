using Core.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Core.Infrastructure.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> contextOptions)
            : base(contextOptions)
        {
        }

        public DbSet<Comment> Comments { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Dossier> Dossiers { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Treatment> Treatments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.Entity<Comment>().ToTable("Comments");
            builder.Entity<Dossier>().ToTable("Dossiers");
            builder.Entity<Treatment>().ToTable("Treatment");
            builder.Entity<TreatmentPlan>().ToTable("TreatmentPlans");
            
            
            builder.Entity<User>().ToTable("Users");
            builder.Entity<Student>().ToTable("Students");
            builder.Entity<Patient>().ToTable("Patients");
            builder.Entity<Doctor>().ToTable("Doctors");

            builder.Entity<Comment>()
                .HasOne( C => C.isPostedOn)
                .WithMany(D => D.Comments)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.Entity<Comment>()
                .HasOne( C => C.CreatedBy)
                .WithMany(U => U.CommentsCreated)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.Entity<Treatment>()
                .HasOne( C => C.Dossier)
                .WithMany(D => D.Treatments)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Treatment>()
                .HasOne( C => C.ExcecutedBy)
                .WithMany(U => U.TreatmentsDone)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.Entity<Dossier>()
                .HasOne( D => D.Patient)
                .WithMany(P => P.Dossiers)
                .OnDelete(DeleteBehavior.NoAction);
            
            builder.Entity<Dossier>()
                .HasOne( D => D.IntakeBy)
                .WithMany(P => P.IntakesDone)
                .OnDelete(DeleteBehavior.NoAction);
            
            builder.Entity<Dossier>()
                .HasOne( D => D.SupervisedBy)
                .WithMany(P => P.IntakesSupervised)
                .OnDelete(DeleteBehavior.NoAction); 
            
            builder.Entity<Dossier>()
                .HasMany<Treatment>( T => T.Treatments)
                .WithOne(T=> T.Dossier)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}