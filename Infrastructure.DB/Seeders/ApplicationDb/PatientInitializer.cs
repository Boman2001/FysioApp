using System;
using Core.Domain.Enums;
using Core.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Infrastructure.Seeders.ApplicationDb
{
    public class PatientInitializer : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> builder)
        {
            builder.ToTable("Patients");

            builder.HasData(new Patient()
                {
                    Id = 3,
                    FirstName = "Paula",
                    Preposition = "van der",
                    LastName = "PatientenBerg",
                    Email = "Paula@vanderpatientenberg.com",
                    PhoneNumber = "0636303815",
                    Gender = Gender.Female,
                    BirthDay = DateTime.Now.AddYears(-56),
                    PatientNumber = Guid.NewGuid().ToString(),
                    PictureUrl = "ee23a151-8ea2-40d6-aad6-9834d3bd4da3_2.jpg"
                },
                new Patient()
                {
                    Id = 4,
                    FirstName = "Pavlov",
                    Preposition = "",
                    LastName = "PatientStan",
                    Email = "Pavlov@PatientStan.com",
                    PhoneNumber = "0636303816",
                    Gender = Gender.Male,
                    BirthDay = DateTime.Now.AddYears(-21),
                    PatientNumber = Guid.NewGuid().ToString(),
                    PictureUrl = "506cf9b3-c437-46bd-944e-3dfcb1d17e8b_9.jpg"
                });
        }
    }
}