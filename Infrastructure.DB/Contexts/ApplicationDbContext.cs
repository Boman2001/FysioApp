using System;
using Core.Domain.Enums;
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
        public DbSet<Staff> Staff { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Dossier> Dossiers { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Treatment> Treatments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.Entity<Comment>().ToTable("Comments");
            builder.Entity<Dossier>().ToTable("Dossiers");
            builder.Entity<Appointment>().HasDiscriminator<bool>("isTreatment")
                .HasValue<Appointment>(false).HasValue<Treatment>(true);
            builder.Entity<TreatmentPlan>().ToTable("TreatmentPlans");
            
            
            builder.Entity<User>().ToTable("Users");
            builder.Entity<Staff>().ToTable("Staff");
            builder.Entity<Student>().ToTable("Students");
            builder.Entity<Patient>().ToTable("Patients");
            builder.Entity<Doctor>().ToTable("Doctors");

            builder.Entity<Comment>()
                .HasOne( comment => comment.isPostedOn)
                .WithMany(dossier => dossier.Comments)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.Entity<Comment>()
                .HasOne( comment => comment.CreatedBy)
                .WithMany(user => user.CommentsCreated)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.Entity<Appointment>()
                .HasOne( treatment => treatment.ExcecutedBy)
                .WithMany(staff => staff.TreatmentsDone)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.Entity<Dossier>()
                .HasOne( dossier => dossier.Patient)
                .WithMany(patient => patient.Dossiers)
                .OnDelete(DeleteBehavior.NoAction);
            
            builder.Entity<Dossier>()
                .HasOne( dossier => dossier.IntakeBy)
                .WithMany(user => user.IntakesDone)
                .OnDelete(DeleteBehavior.NoAction);
            
            builder.Entity<Dossier>()
                .HasOne( dossier => dossier.SupervisedBy)
                .WithMany(user => user.IntakesSupervised)
                .OnDelete(DeleteBehavior.NoAction); 
            
            builder.Entity<Dossier>()
                .HasMany<Appointment>( T => T.Appointments)
                .WithOne(T=> T.Dossier)
                .OnDelete(DeleteBehavior.Cascade);

            // builder.Entity<Student>().HasData(new Student()
            // {
            //     FirstName = "Stefan",
            //     Preposition = "De",
            //     LastName = "Student",
            //     Email = "Danmaarkaas@gmail.com",
            //     StudentNumber = "2153494",
            //     start = new TimeSpan(0),
            //         end = new TimeSpan(0)
            // });
            //
            // builder.Entity<Doctor>().HasData(new Doctor()   
            // {
            //     FirstName = "Dirk",
            //     Preposition = "De",
            //     LastName = "DoctorMan",
            //     Email = "Danmaarkaas1@gmail.com",
            //      BigNumber = "29292929929",
            //     start = new TimeSpan(0),
            //     end = new TimeSpan(0),
            //     PhoneNumber = "0636303815",
            //      EmployeeNumber = "0636303815"
            // });
            //
            // builder.Entity<Patient>().HasData(new Patient()   
            // {
            //     FirstName = "Paula",
            //     Preposition = "van der",
            //     LastName = "PatientenBerg",
            //     Email = "Danmaarkaas2@gmail.com",
            //     PhoneNumber = "0636303815",
            //     Gender = Gender.Female,
            //     BirthDay = DateTime.Now.AddYears(-56),
            //     PatientNumber = Guid.NewGuid().ToString(),
            //     PictureUrl = "ee23a151-8ea2-40d6-aad6-9834d3bd4da3_2.jpg"
            // });
            //
            // builder.Entity<Patient>().HasData(new Patient()   
            // {
            //     FirstName = "Pavlov",
            //     Preposition = "",
            //     LastName = "PatientStan",
            //     Email = "Danmaarkaas3@gmail.com",
            //     PhoneNumber = "0636303816",
            //     Gender = Gender.Male,
            //     BirthDay = DateTime.Now.AddYears(-21),
            //     PatientNumber = Guid.NewGuid().ToString(),
            //     PictureUrl = "ee23a151-8ea2-40d6-aad6-9834d3bd4da3_2.jpg"
            // });
            
        }
    }
}