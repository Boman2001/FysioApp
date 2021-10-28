using System;
using Core.Domain.Enums;
using Core.Domain.Models;
using Core.Infrastructure.Seeders.ApplicationDb;
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
            builder.ApplyConfiguration(new AppointmentInitializer());
            builder.ApplyConfiguration(new CommentInitializer());
            builder.ApplyConfiguration(new DoctorInitalizer());
            builder.ApplyConfiguration(new DossierInitializer());
            builder.ApplyConfiguration(new PatientInitializer());
            builder.ApplyConfiguration(new StaffInitializer());
            builder.ApplyConfiguration(new StudentInitializer());
            builder.ApplyConfiguration(new TreatmentPlanInitializer());
            builder.ApplyConfiguration(new UserInitializer());
        }
    }
}