using System;
using Core.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Infrastructure.Seeders.ApplicationDb
{
    public class StudentInitializer : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.ToTable("Students");
            builder.HasData(new Student()
            {
                Id = 2,
                FirstName = "Stefan",
                Preposition = "De",
                LastName = "Student",
                Email = "stefan@DeStudent.com",
                StudentNumber = "2153494",
                start = new TimeSpan(0),
                end = new TimeSpan(0)
            }); 
        }
    }
}