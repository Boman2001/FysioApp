using System;
using System.Collections.Generic;
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
    public class TreatmentServiceTests
    {
        private Mock<IRepository<Treatment>> repository;

        public TreatmentServiceTests()
        {
            repository = new Mock<IRepository<Treatment>>();
            Mock<Dossier> dossierMock = new Mock<Dossier>();
            Mock<Staff> staffMock = new Mock<Staff>();
            repository.Setup(c => c.Add(It.IsAny<Treatment>())).ReturnsAsync(() => { return new Treatment()
            {
                Id = 0,
                Description = "description",
                Dossier = dossierMock.Object,
                Particulatities = "",
                Room = RoomType.None,
                CreatedAt = DateTime.Now,
                ExcecutedBy = staffMock.Object,
                TreatmentCode = new TreatmentCode()
                {
                    Code  = "",
                    Description = "",
                    Id = 0,
                    ExplanationRequired = false
                },
                TreatmentDate = DateTime.Now,
                TreatmentCodeId = 5,
                TreatmentEndDate = DateTime.Now.AddDays(1)
            };});
            repository.Setup(c => c.Update(It.IsAny<Treatment>())).ReturnsAsync(() => { return new Treatment()
            {
                Id = 0,
                Description = "description",
                Dossier = dossierMock.Object,
                Particulatities = "",
                Room = RoomType.None,
                CreatedAt = DateTime.Now,
                ExcecutedBy = staffMock.Object,
                TreatmentCode = new TreatmentCode()
                {
                    Code  = "",
                    Description = "",
                    Id = 0,
                    ExplanationRequired = false
                },
                TreatmentDate = DateTime.Now,
                TreatmentCodeId = 5,
                TreatmentEndDate = DateTime.Now.AddDays(1)
            };});

        }


        [Fact]
        public async Task TreatmentService_Create_must_error_when_description_is_needed_and_not_given()
        {
            // Arrange
            TreatmentService sut = new TreatmentService(repository.Object);
            Mock<Dossier> dossierMock = new Mock<Dossier>();
            Mock<Staff> staffMock = new Mock<Staff>();
            Treatment treatment = new Treatment()
            {
                Id = 0,
                Description = "",
                Dossier = dossierMock.Object,
                Particulatities = "",
                Room = RoomType.None,
                CreatedAt = DateTime.Now,
                ExcecutedBy = staffMock.Object,
                TreatmentCode = new TreatmentCode()
                {
                  Code  = "",
                  Description = "",
                  Id = 0,
                  ExplanationRequired = true
                },
                TreatmentDate = DateTime.Now,
                TreatmentCodeId = 5,
                TreatmentEndDate = DateTime.Now.AddDays(1)
            };


            // Act
            Func<Task> act = () =>  sut.Add(treatment);
            
            // Assert
            ValidationException exception =   await Assert.ThrowsAsync<ValidationException>(act);
            
            Assert.Equal("Voor dit type behandeling is een beschrijving verplicht", exception.Message);
        }
        
        [Fact]
        public async Task TreatmentService_Create_must_not_error_when_description_is_needed_and_given()
        {
            // Arrange
            TreatmentService sut = new TreatmentService(repository.Object);
            Mock<Dossier> dossierMock = new Mock<Dossier>();
            Mock<Staff> staffMock = new Mock<Staff>();
            Treatment treatment = new Treatment()
            {
                Id = 0,
                Description = "description",
                Dossier = dossierMock.Object,
                Particulatities = "",
                Room = RoomType.None,
                CreatedAt = DateTime.Now,
                ExcecutedBy = staffMock.Object,
                TreatmentCode = new TreatmentCode()
                {
                    Code  = "",
                    Description = "",
                    Id = 0,
                    ExplanationRequired = true
                },
                TreatmentDate = DateTime.Now,
                TreatmentCodeId = 5,
                TreatmentEndDate = DateTime.Now.AddDays(1)
            };


            // Act
            var result = await sut.Add(treatment);
            
            // Assert
            Assert.Equal(result.Id, treatment.Id);
        }
        
        [Fact]
        public async Task TreatmentService_Create_must_not_error_when_description_is_not_needed_and_given()
        {
            // Arrange
            TreatmentService sut = new TreatmentService(repository.Object);
            Mock<Dossier> dossierMock = new Mock<Dossier>();
            Mock<Staff> staffMock = new Mock<Staff>();
            staffMock.Setup(u => u.HeadPractisionerOf).Returns(() => new List<Dossier>() {dossierMock.Object});
            dossierMock.Setup(d => d.Treatments).Returns(() => new List<Treatment>());
            dossierMock.Setup(d => d.Appointments).Returns(() => new List<Appointment>());
            Treatment treatment = new Treatment()
            {
                Id = 0,
                Description = "description",
                Dossier = dossierMock.Object,
                Particulatities = "",
                Room = RoomType.None,
                CreatedAt = DateTime.Now,
                ExcecutedBy = staffMock.Object,
                TreatmentCode = new TreatmentCode()
                {
                    Code  = "",
                    Description = "",
                    Id = 0,
                    ExplanationRequired = false
                },
                TreatmentDate = DateTime.Now,
                TreatmentCodeId = 5,
                TreatmentEndDate = DateTime.Now.AddDays(1)
            };


            // Act
            var result = await sut.Add(treatment);
            
            // Assert
            Assert.Equal(result.Id, treatment.Id);
        }
        
        [Fact]
        public async Task TreatmentService_Create_must_not_error_when_description_is_not_needed_and_not_given()
        {
            // Arrange
            TreatmentService sut = new TreatmentService(repository.Object);
            Mock<Dossier> dossierMock = new Mock<Dossier>();
            Mock<Staff> staffMock = new Mock<Staff>();
            Treatment treatment = new Treatment()
            {
                Id = 0,
                Description = "",
                Dossier = dossierMock.Object,
                Particulatities = "",
                Room = RoomType.None,
                CreatedAt = DateTime.Now,
                ExcecutedBy = staffMock.Object,
                TreatmentCode = new TreatmentCode()
                {
                    Code  = "",
                    Description = "",
                    Id = 0,
                    ExplanationRequired = false
                },
                TreatmentDate = DateTime.Now,
                TreatmentCodeId = 5,
                TreatmentEndDate = DateTime.Now.AddDays(1)
            };


            // Act
            var result = await sut.Add(treatment);
            
            // Assert
            Assert.Equal(result.Id, treatment.Id);
        }
        
                [Fact]
        public async Task TreatmentService_Update_must_error_when_description_is_needed_and_not_given()
        {
            // Arrange
            TreatmentService sut = new TreatmentService(repository.Object);
            Mock<Dossier> dossierMock = new Mock<Dossier>();
            Mock<Staff> staffMock = new Mock<Staff>();
            Treatment treatment = new Treatment()
            {
                Id = 0,
                Description = "",
                Dossier = dossierMock.Object,
                Particulatities = "",
                Room = RoomType.None,
                CreatedAt = DateTime.Now,
                ExcecutedBy = staffMock.Object,
                TreatmentCode = new TreatmentCode()
                {
                  Code  = "",
                  Description = "",
                  Id = 0,
                  ExplanationRequired = true
                },
                TreatmentDate = DateTime.Now,
                TreatmentCodeId = 5,
                TreatmentEndDate = DateTime.Now.AddDays(1)
            };


            // Act
            Func<Task> act = () =>  sut.Update(treatment);
            
            // Assert
            ValidationException exception =   await Assert.ThrowsAsync<ValidationException>(act);
            
            Assert.Equal("Voor dit type behandeling is een beschrijving verplicht", exception.Message);
        }
        
        [Fact]
        public async Task TreatmentService_Update_must_not_error_when_description_is_needed_and_given()
        {
            // Arrange
            TreatmentService sut = new TreatmentService(repository.Object);
            Mock<Dossier> dossierMock = new Mock<Dossier>();
            Mock<Staff> staffMock = new Mock<Staff>();
            Treatment treatment = new Treatment()
            {
                Id = 0,
                Description = "description",
                Dossier = dossierMock.Object,
                Particulatities = "",
                Room = RoomType.None,
                CreatedAt = DateTime.Now,
                ExcecutedBy = staffMock.Object,
                TreatmentCode = new TreatmentCode()
                {
                    Code  = "",
                    Description = "",
                    Id = 0,
                    ExplanationRequired = true
                },
                TreatmentDate = DateTime.Now,
                TreatmentCodeId = 5,
                TreatmentEndDate = DateTime.Now.AddDays(1)
            };


            // Act
            var result = await sut.Update(treatment);
            
            // Assert
            Assert.Equal(result.Id, treatment.Id);
        }
        
        [Fact]
        public async Task TreatmentService_Update_must_not_error_when_description_is_not_needed_and_given()
        {
            // Arrange
            TreatmentService sut = new TreatmentService(repository.Object);
            Mock<Dossier> dossierMock = new Mock<Dossier>();
            Mock<Staff> staffMock = new Mock<Staff>();
            Treatment treatment = new Treatment()
            {
                Id = 0,
                Description = "description",
                Dossier = dossierMock.Object,
                Particulatities = "",
                Room = RoomType.None,
                CreatedAt = DateTime.Now,
                ExcecutedBy = staffMock.Object,
                TreatmentCode = new TreatmentCode()
                {
                    Code  = "",
                    Description = "",
                    Id = 0,
                    ExplanationRequired = false
                },
                TreatmentDate = DateTime.Now,
                TreatmentCodeId = 5,
                TreatmentEndDate = DateTime.Now.AddDays(1)
            };


            // Act
            var result = await sut.Update(treatment);
            
            // Assert
            Assert.Equal(result.Id, treatment.Id);
        }
        
        [Fact]
        public async Task TreatmentService_Update_must_not_error_when_description_is_not_needed_and_not_given()
        {
            // Arrange
            TreatmentService sut = new TreatmentService(repository.Object);
            Mock<Dossier> dossierMock = new Mock<Dossier>();
            Mock<Staff> staffMock = new Mock<Staff>();
            Treatment treatment = new Treatment()
            {
                Id = 0,
                Description = "",
                Dossier = dossierMock.Object,
                Particulatities = "",
                Room = RoomType.None,
                CreatedAt = DateTime.Now,
                ExcecutedBy = staffMock.Object,
                TreatmentCode = new TreatmentCode()
                {
                    Code  = "",
                    Description = "",
                    Id = 0,
                    ExplanationRequired = false
                },
                TreatmentDate = DateTime.Now,
                TreatmentCodeId = 5,
                TreatmentEndDate = DateTime.Now.AddDays(1)
            };


            // Act
            var result = await sut.Update(treatment);
            
            // Assert
            Assert.Equal(result.Id, treatment.Id);
        }

    }
}