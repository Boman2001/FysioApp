using System;
using System.Threading.Tasks;
using ApplicationServices.Services;
using Core.Domain.Enums;
using Core.Domain.Exceptions;
using Core.Domain.Models;
using Core.DomainServices.Interfaces;
using Moq;
using Xunit;

namespace ApplicationServicesTests.Services
{
    public class PatientServiceTest
    {
        private Mock<IRepository<Patient>> repository;

        public PatientServiceTest()
        {
            repository = new Mock<IRepository<Patient>>();
            repository.Setup(c => c.Add(It.IsAny<Patient>())).ReturnsAsync(() =>
            {
                return new Patient()
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
                };
            });

            repository.Setup(c => c.Update(It.IsAny<Patient>())).ReturnsAsync(() =>
            {
                return new Patient()
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
                };
            });
        }

        [Fact]
        public async Task PatientService_Create_must_error_if_Patient_Younger_than_16()
        {
            // Arrange
            PatientService sut = new PatientService(repository.Object);
            Patient patient = new Patient()
            {
                Id = 3,
                FirstName = "Paula",
                Preposition = "van der",
                LastName = "PatientenBerg",
                Email = "Paula@vanderpatientenberg.com",
                PhoneNumber = "0636303815",
                Gender = Gender.Female,
                BirthDay = DateTime.Now.AddYears(-15),
                PatientNumber = Guid.NewGuid().ToString(),
                PictureUrl = "ee23a151-8ea2-40d6-aad6-9834d3bd4da3_2.jpg"
            };

            // Act
            Func<Task> act = () => sut.Add(patient);

            // Assert
            ValidationException exception = await Assert.ThrowsAsync<ValidationException>(act);

            Assert.Equal("Patient is not old enough must be atleast 16", exception.Message);
        }
        
        [Fact]
        public async Task PatientService_Create_must_not_error_if_Patient_Younger_than_16()
        {
            // Arrange
            PatientService sut = new PatientService(repository.Object);
            Patient patient = new Patient()
            {
                Id = 3,
                FirstName = "Paula",
                Preposition = "van der",
                LastName = "PatientenBerg",
                Email = "Paula@vanderpatientenberg.com",
                PhoneNumber = "0636303815",
                Gender = Gender.Female,
                BirthDay = DateTime.Now.AddYears(-66),
                PatientNumber = Guid.NewGuid().ToString(),
                PictureUrl = "ee23a151-8ea2-40d6-aad6-9834d3bd4da3_2.jpg"
            };

            // Act
            var result = await sut.Add(patient);

            // Assert
            Assert.Equal(result.Id, patient.Id);
        }
        
                [Fact]
                public async Task PatientService_Update_must_error_if_Patient_Younger_than_16()
                {
                    // Arrange
                    PatientService sut = new PatientService(repository.Object);
                    Patient patient = new Patient()
                    {
                        Id = 3,
                        FirstName = "Paula",
                        Preposition = "van der",
                        LastName = "PatientenBerg",
                        Email = "Paula@vanderpatientenberg.com",
                        PhoneNumber = "0636303815",
                        Gender = Gender.Female,
                        BirthDay = DateTime.Now.AddYears(-15),
                        PatientNumber = Guid.NewGuid().ToString(),
                        PictureUrl = "ee23a151-8ea2-40d6-aad6-9834d3bd4da3_2.jpg"
                    };
        
                    // Act
                    Func<Task> act = () => sut.Update(patient);
        
                    // Assert
                    ValidationException exception = await Assert.ThrowsAsync<ValidationException>(act);
        
                    Assert.Equal("Patient is not old enough must be atleast 16", exception.Message);
                }
                
                [Fact]
                public async Task PatientService_Update_must_not_error_if_Patient_Younger_than_16()
                {
                    // Arrange
                    PatientService sut = new PatientService(repository.Object);
                    Patient patient = new Patient()
                    {
                        Id = 3,
                        FirstName = "Paula",
                        Preposition = "van der",
                        LastName = "PatientenBerg",
                        Email = "Paula@vanderpatientenberg.com",
                        PhoneNumber = "0636303815",
                        Gender = Gender.Female,
                        BirthDay = DateTime.Now.AddYears(-66),
                        PatientNumber = Guid.NewGuid().ToString(),
                        PictureUrl = "ee23a151-8ea2-40d6-aad6-9834d3bd4da3_2.jpg"
                    };
        
                    // Act
                    var result = await sut.Update(patient);
        
                    // Assert
                    Assert.Equal(result.Id, patient.Id);
                }
    }
}