using System;
using Core.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Infrastructure.Seeders.ApplicationDb
{
    public class DoctorInitalizer : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {
            builder.ToTable("Doctors");
            
            builder.HasData(new Doctor()   
            {
                Id = 1,
                FirstName = "Drik",
                Preposition = "De",
                LastName = "DoctorMan",
                Email = "Drik@deDoktor.com",
                BigNumber = "29292929929",
                start = new TimeSpan(0),
                end = new TimeSpan(0),
                PhoneNumber = "0636303815",
                EmployeeNumber = "0636303815"
            });

        }
    }
}