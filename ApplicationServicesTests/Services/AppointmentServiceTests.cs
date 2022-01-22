using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using ApplicationServices.ExtensionMethods;
using ApplicationServices.Helpers;
using ApplicationServices.Services;
using Core.Domain.Enums;
using Core.Domain.Exceptions;
using Core.Domain.Models;
using Core.DomainServices.Interfaces;
using Moq;
using TypeMock.ArrangeActAssert;
using Xunit;

namespace ApplicationServicesTests.Services
{
    public class AppointmentServiceTests
    {
        private Mock<IAppointmentRepository> repository;
        private readonly Mock<IRepository<TreatmentPlan>> _treatmentPlanRepository;
        private readonly Mock<IService<Dossier>> _dossierService;
        private readonly Mock<IDatetimeHelper> _datetimeHelper;

        public AppointmentServiceTests()
        {
            repository = new Mock<IAppointmentRepository>();
            _treatmentPlanRepository = new Mock<IRepository<TreatmentPlan>>();
            _dossierService = new Mock<IService<Dossier>>();
            Mock<Dossier> dossierMock = new Mock<Dossier>();
            Mock<Staff> staffMock = new Mock<Staff>();
            _datetimeHelper =  new Mock<IDatetimeHelper>();
            _datetimeHelper.Setup(fake => fake.Now())
                .Returns(new DateTime(2025,01,20,9,0,0));
            repository.Setup(c => c.Add(It.IsAny<Appointment>())).ReturnsAsync(() =>
            {
                return new Appointment()
                {
                    Id = 0,
                    Dossier = dossierMock.Object,
                    Room = RoomType.None,
                    CreatedAt = new DateTime(2025,01,20,9,0,0),
                    ExcecutedBy = staffMock.Object,
                    TreatmentDate = new DateTime(2025,01,20,9,0,0),
                    TreatmentEndDate = new DateTime(2025,01,20,9,0,0).AddDays(1)
                };
            });
            
            repository.Setup(c => c.Update(It.IsAny<Appointment>())).ReturnsAsync(() =>
            {
                return new Appointment()
                {
                    Id = 0,
                    Dossier = dossierMock.Object,
                    Room = RoomType.None,
                    CreatedAt = new DateTime(2025,01,20,9,0,0),
                    ExcecutedBy = staffMock.Object,
                    TreatmentDate = new DateTime(2025,01,20,9,0,0),
                    TreatmentEndDate = new DateTime(2025,01,20,9,0,0).AddDays(1)
                };
            });
            repository.Setup(c => c.Update(It.IsAny<Appointment>())).ReturnsAsync(() =>
            {
                return new Appointment()
                {
                    Id = 0,
                    Dossier = dossierMock.Object,
                    Room = RoomType.None,
                    CreatedAt = new DateTime(2025,01,20,9,0,0),
                    ExcecutedBy = staffMock.Object,
                    TreatmentDate = new DateTime(2025,01,20,9,0,0),
                    TreatmentEndDate = new DateTime(2025,01,20,9,0,0).AddDays(1)
                };
            });

            _dossierService.Setup(c => c.Get(It.IsAny<int>())).ReturnsAsync(() =>
            {
                return new Dossier()
                {
                    Age = 12,
                    Appointments = new List<Appointment>(),
                    Comments = new List<Comment>(),
                    Description = "",
                    Patient = new Patient(),
                    Id = 0,
                    Treatments = new List<Treatment>(),
                    CreatedAt = new DateTime(2025,01,20,9,0,0),
                    DiagnoseCode = new DiagnoseCode(),
                    HeadPractitioner = new Staff(),
                    IntakeBy = new Staff(),
                    IsStudent = true,
                    TreatmentPlan = new TreatmentPlan(),
                    DiagnoseCodeId = 0,
                };
            });
        }


        [Fact]
        public async Task AppointmentService_Create_must_error_when_Max_Amount_Of_Treatments_In_Week_Is_Surpassed()
        {
            // Arrange
            AppointmentService sut = new AppointmentService(repository.Object, _dossierService.Object,
                _treatmentPlanRepository.Object, _datetimeHelper.Object);
            Mock<Dossier> dossierMock = new Mock<Dossier>();
            dossierMock.Setup(d => d.TreatmentPlan).Returns(new TreatmentPlan()
            {
                Id = 1,
                CreatedAt = new DateTime(2025,01,20,9,0,0),
                TreatmentsPerWeek = 5,
                TimePerSessionInMinutes = 50
            });
            Staff staff = new Staff()
            {
                start = new TimeSpan(0, 0, 0),
                end = new TimeSpan(24, 0, 0),
                Id = 0,
                Email = "email",
                CreatedAt = new DateTime(2025,01,20,9,0,0),
                FirstName = "",
                CommentsCreated = new List<Comment>(),
                IntakesDone = new List<Dossier>(),
                IntakesSupervised = new List<Dossier>(),
                LastName = "",
                HeadPractisionerOf = new List<Dossier>() {dossierMock.Object}
            };
            _treatmentPlanRepository.Setup(c => c.Get(It.IsAny<int>())).ReturnsAsync(() =>
            {
                return new TreatmentPlan()
                {
                    Id = 0,
                    CreatedAt = new DateTime(2025,01,20,9,0,0),
                    TreatmentsPerWeek = 0,
                    TimePerSessionInMinutes = 50
                };
            });

            Appointment treatment = new Appointment()
            {
                Id = 0,
                Dossier = dossierMock.Object,
                Room = RoomType.None,
                CreatedAt = new DateTime(2025,01,20,9,0,0),
                ExcecutedBy = staff,
                TreatmentDate = new DateTime(2025,01,20,9,0,0).AddMinutes(5),
                TreatmentEndDate = new DateTime(2025,01,20,9,0,0).AddDays(1),
            };

            // Act
            Func<Task> act = () => sut.Add(treatment);

            // Assert
            ValidationException exception = await Assert.ThrowsAsync<ValidationException>(act);

            Assert.Equal("Het maximum aantal afspraken zijn al aangemaakt voor deze week", exception.Message);
        }

        [Fact]
        public async Task AppointmentService_Create_must_error_when_OutsideWOrkHours()
        {
            // Arrange
            AppointmentService sut = new AppointmentService(repository.Object, _dossierService.Object,
                _treatmentPlanRepository.Object, _datetimeHelper.Object);
            Mock<Dossier> dossierMock = new Mock<Dossier>();
            dossierMock.Setup(d => d.TreatmentPlan).Returns(new TreatmentPlan()
            {
                Id = 1,
                CreatedAt = new DateTime(2025,01,20,9,0,0),
                TreatmentsPerWeek = 5,
                TimePerSessionInMinutes = 50
            });
            Staff staff = new Staff()
            {
                start = new TimeSpan(0, 0, 0),
                end = new TimeSpan(0, 0, 0),
                Id = 0,
                Email = "email",
                CreatedAt = new DateTime(2025,01,20,9,0,0),
                FirstName = "",
                CommentsCreated = new List<Comment>(),
                IntakesDone = new List<Dossier>(),
                IntakesSupervised = new List<Dossier>(),
                LastName = "",
                HeadPractisionerOf = new List<Dossier>() {dossierMock.Object}
            };
            _treatmentPlanRepository.Setup(c => c.Get(It.IsAny<int>())).ReturnsAsync(() =>
            {
                return new TreatmentPlan()
                {
                    Id = 0,
                    CreatedAt = new DateTime(2025,01,20,9,0,0),
                    TreatmentsPerWeek = 1,
                    TimePerSessionInMinutes = 50
                };
            });

            Appointment treatment = new Appointment()
            {
                Id = 0,
                Dossier = dossierMock.Object,
                Room = RoomType.None,
                CreatedAt = new DateTime(2025,01,20,9,0,0),
                ExcecutedBy = staff,
                TreatmentDate = new DateTime(2025,01,20,9,0,0).AddMinutes(5),
                TreatmentEndDate = new DateTime(2025,01,20,9,0,0).AddDays(1),
            };
            // Act
            Func<Task> act = () => sut.Add(treatment);

            // Assert
            ValidationException exception = await Assert.ThrowsAsync<ValidationException>(act);

            Assert.Equal("Deze behandelinmg valt buiten de werktijden van uw doctor", exception.Message);
        }

        [Fact]
        public async Task AppointmentService_Create_must_error_when_other_appointment_is_planned()
        {
            // Arrange
            AppointmentService sut = new AppointmentService(repository.Object, _dossierService.Object,
                _treatmentPlanRepository.Object, _datetimeHelper.Object);
            Mock<Dossier> dossierMock = new Mock<Dossier>();
            dossierMock.Setup(d => d.TreatmentPlan).Returns(new TreatmentPlan()
            {
                Id = 1,
                CreatedAt = new DateTime(2025,01,20,9,0,0),
                TreatmentsPerWeek = 5,
                TimePerSessionInMinutes = 50
            });
            Staff staff = new Staff()
            {
                start = new TimeSpan(0, 0, 0),
                end = new TimeSpan(24, 0, 0),
                Id = 0,
                Email = "email",
                CreatedAt = new DateTime(2025,01,20,9,0,0),
                FirstName = "",
                CommentsCreated = new List<Comment>(),
                IntakesDone = new List<Dossier>(),
                IntakesSupervised = new List<Dossier>(),
                LastName = "",
                HeadPractisionerOf = new List<Dossier>() {dossierMock.Object}
            };
            _treatmentPlanRepository.Setup(c => c.Get(It.IsAny<int>())).ReturnsAsync(() =>
            {
                return new TreatmentPlan()
                {
                    Id = 0,
                    CreatedAt = new DateTime(2025,01,20,9,0,0),
                    TreatmentsPerWeek = 1,
                    TimePerSessionInMinutes = 50
                };
            });
            dossierMock.Setup(d => d.Appointments).Returns(() =>
            {
                return new List<Appointment>()
                {
                    new Appointment()
                    {
                        Id = 0,
                        Dossier = dossierMock.Object,
                        Room = RoomType.None,
                        CreatedAt = new DateTime(2025,01,20,9,0,0),
                        ExcecutedBy = staff,
                        TreatmentDate = new DateTime(2025,01,20,9,0,0).AddMinutes(5),
                        TreatmentEndDate = new DateTime(2025,01,20,9,0,0).AddHours(2),
                    }
                };
            });


            Appointment treatment = new Appointment()
            {
                Id = 0,
                Dossier = dossierMock.Object,
                Room = RoomType.None,
                CreatedAt = new DateTime(2025,01,20,9,0,0),
                ExcecutedBy = staff,
                TreatmentDate = new DateTime(2025,01,20,9,0,0).AddMinutes(10),
                TreatmentEndDate = new DateTime(2025,01,20,9,0,0).AddHours(1),
            };
            // Act
            Func<Task> act = () => sut.Add(treatment);

            // Assert
            ValidationException exception = await Assert.ThrowsAsync<ValidationException>(act);

            Assert.Equal("Uw doctor is al bezet op dit moment", exception.Message);
        }

        [Fact]
        public async Task AppointmentService_Create_must_error_when_appointment_is_planned_outside_treatment_period()
        {
            // Arrange
            AppointmentService sut = new AppointmentService(repository.Object, _dossierService.Object,
                _treatmentPlanRepository.Object, _datetimeHelper.Object);
            Mock<Dossier> dossierMock = new Mock<Dossier>();
            dossierMock.Setup(d => d.TreatmentPlan).Returns(new TreatmentPlan()
            {
                Id = 1,
                CreatedAt = new DateTime(2025,01,20,9,0,0),
                TreatmentsPerWeek = 5,
                TimePerSessionInMinutes = 50
            });

            Staff staff = new Staff()
            {
                start = new TimeSpan(0, 0, 0),
                end = new TimeSpan(24, 0, 0),
                Id = 0,
                Email = "email",
                CreatedAt = new DateTime(2025,01,20,9,0,0),
                FirstName = "",
                CommentsCreated = new List<Comment>(),
                IntakesDone = new List<Dossier>(),
                IntakesSupervised = new List<Dossier>(),
                LastName = "",
                HeadPractisionerOf = new List<Dossier>() {dossierMock.Object}
            };
            _treatmentPlanRepository.Setup(c => c.Get(It.IsAny<int>())).ReturnsAsync(() =>
            {
                return new TreatmentPlan()
                {
                    Id = 0,
                    CreatedAt = new DateTime(2025,01,20,9,0,0),
                    TreatmentsPerWeek = 1,
                    TimePerSessionInMinutes = 50
                };
            });


            Appointment treatment = new Appointment()
            {
                Id = 0,
                Dossier = new Dossier()
                {
                    RegistrationDate = new DateTime(2025,01,20,9,0,0).AddDays(2),
                    DismissionDate = new DateTime(2025,01,20,9,0,0).AddDays(3),
                    HeadPractitioner = staff,
                    TreatmentPlan = new TreatmentPlan()
                    {
                        Id = 1,
                        CreatedAt = new DateTime(2025,01,20,9,0,0),
                        TreatmentsPerWeek = 5,
                        TimePerSessionInMinutes = 50
                    },
                },
                Room = RoomType.None,
                CreatedAt = new DateTime(2025,01,20,9,0,0),
                ExcecutedBy = staff,
                TreatmentDate = new DateTime(2025,01,20,9,0,0).AddMinutes(5),
                TreatmentEndDate = new DateTime(2025,01,20,9,0,0).AddHours(1),
            };
            // Act
            Func<Task> act = () => sut.Add(treatment);

            // Assert
            ValidationException exception = await Assert.ThrowsAsync<ValidationException>(act);

            Assert.Equal("een behandeling kan niet geplanned worden buiten een behandel periode", exception.Message);
        }

        [Fact]
        public async Task AppointmentService_remove_must_error_when_called_less_than_24H_before_start()
        {
            // Arrange
            
            AppointmentService sut = new AppointmentService(repository.Object, _dossierService.Object,
                _treatmentPlanRepository.Object, _datetimeHelper.Object);
            Mock<Dossier> dossierMock = new Mock<Dossier>();
            dossierMock.Setup(d => d.TreatmentPlan).Returns(new TreatmentPlan()
            {
                Id = 1,
                CreatedAt = new DateTime(2025,01,20,9,0,0),
                TreatmentsPerWeek = 5,
                TimePerSessionInMinutes = 50
            });
            
            

            Staff staff = new Staff()
            {
                start = new TimeSpan(0, 0, 0),
                end = new TimeSpan(24, 0, 0),
                Id = 0,
                Email = "email",
                CreatedAt = new DateTime(2025,01,20,9,0,0),
                FirstName = "",
                CommentsCreated = new List<Comment>(),
                IntakesDone = new List<Dossier>(),
                IntakesSupervised = new List<Dossier>(),
                LastName = "",
                HeadPractisionerOf = new List<Dossier>() {dossierMock.Object}
            };
            _treatmentPlanRepository.Setup(c => c.Get(It.IsAny<int>())).ReturnsAsync(() =>
            {
                return new TreatmentPlan()
                {
                    Id = 0,
                    CreatedAt = new DateTime(2025,01,20,9,0,0),
                    TreatmentsPerWeek = 1,
                    TimePerSessionInMinutes = 50
                };
            });


            Appointment treatment = new Appointment()
            {
                Id = 0,
                Dossier = new Dossier()
                {
                    RegistrationDate = new DateTime(2025,01,20,9,0,0).AddDays(2),
                    DismissionDate = new DateTime(2025,01,20,9,0,0).AddDays(3),
                    HeadPractitioner = staff,
                    TreatmentPlan = new TreatmentPlan()
                    {
                        Id = 1,
                        CreatedAt = new DateTime(2025,01,20,9,0,0),
                        TreatmentsPerWeek = 5,
                        TimePerSessionInMinutes = 50
                    },
                },
                Room = RoomType.None,
                CreatedAt = new DateTime(2025,01,20,9,0,0),
                ExcecutedBy = staff,
                TreatmentDate = new DateTime(2025,01,20,9,0,0).AddMinutes(5),
                TreatmentEndDate = new DateTime(2025,01,20,9,0,0).AddHours(1),
            };
            // Act
            Func<Task> act = () => sut.Delete(treatment);

            // Assert
            ValidationException exception = await Assert.ThrowsAsync<ValidationException>(act);

            Assert.Equal("een behandeling kan niet verwijderd worden binnen 24 uur van het begin van de afspraak",
                exception.Message);
        }

        [Fact]
        public async Task
            AppointmentService_Create_must_not_error_when_Max_Amount_Of_Treatments_In_Week_Is_Not_Surpassed()
        {
            // Arrange
            AppointmentService sut = new AppointmentService(repository.Object, _dossierService.Object,
                _treatmentPlanRepository.Object, _datetimeHelper.Object);
            Mock<Dossier> dossierMock = new Mock<Dossier>();
            dossierMock.Setup(d => d.TreatmentPlan).Returns(new TreatmentPlan()
            {
                Id = 1,
                CreatedAt = new DateTime(2025,01,20,9,0,0),
                TreatmentsPerWeek = 5,
                TimePerSessionInMinutes = 50
            });
            Staff staff = new Staff()
            {
                start = new TimeSpan(0, 0, 0),
                end = new TimeSpan(24, 0, 0),
                Id = 0,
                Email = "email",
                CreatedAt = new DateTime(2025,01,20,9,0,0),
                FirstName = "",
                CommentsCreated = new List<Comment>(),
                IntakesDone = new List<Dossier>(),
                IntakesSupervised = new List<Dossier>(),
                LastName = "",
                HeadPractisionerOf = new List<Dossier>() {dossierMock.Object}
            };
            _treatmentPlanRepository.Setup(c => c.Get(It.IsAny<int>())).ReturnsAsync(() =>
            {
                return new TreatmentPlan()
                {
                    Id = 0,
                    CreatedAt = new DateTime(2025,01,20,9,0,0),
                    TreatmentsPerWeek = 1,
                    TimePerSessionInMinutes = 50
                };
            });

            Appointment treatment = new Appointment()
            {
                Id = 0,
                Dossier = new Dossier()
                {
                    RegistrationDate = new DateTime(2025,01,20,9,0,0).AddDays(-2),
                    DismissionDate = new DateTime(2025,01,20,9,0,0).AddDays(3),
                    HeadPractitioner = staff,
                    TreatmentPlan = new TreatmentPlan()
                    {
                        Id = 1,
                        CreatedAt = new DateTime(2025,01,20,9,0,0),
                        TreatmentsPerWeek = 5,
                        TimePerSessionInMinutes = 50
                    },
                },
                Room = RoomType.None,
                CreatedAt = new DateTime(2025,01,20,9,0,0),
                ExcecutedBy = staff,
                TreatmentDate = new DateTime(2025,01,20,9,0,0).AddMinutes(5),
                TreatmentEndDate = new DateTime(2025,01,20,9,0,0).AddDays(1),
            };
            // Act
            var result = await sut.Add(treatment);

            // Assert
            Assert.Equal(result.Id, treatment.Id);
        }

        [Fact]
        public async Task
            AppointmentService_Create_must_Not_error_when_Max_Amount_Of_Treatments_In_Week_Is_not_Surpassed()
        {
            // Arrange
            AppointmentService sut = new AppointmentService(repository.Object, _dossierService.Object,
                _treatmentPlanRepository.Object, _datetimeHelper.Object);
            Mock<Dossier> dossierMock = new Mock<Dossier>();
            dossierMock.Setup(d => d.TreatmentPlan).Returns(new TreatmentPlan()
            {
                Id = 1,
                CreatedAt = new DateTime(2025,01,20,9,0,0),
                TreatmentsPerWeek = 5,
                TimePerSessionInMinutes = 50
            });
            Staff staff = new Staff()
            {
                start = new TimeSpan(0, 0, 0),
                end = new TimeSpan(24, 0, 0),
                Id = 0,
                Email = "email",
                CreatedAt = new DateTime(2025,01,20,9,0,0),
                FirstName = "",
                CommentsCreated = new List<Comment>(),
                IntakesDone = new List<Dossier>(),
                IntakesSupervised = new List<Dossier>(),
                LastName = "",
                HeadPractisionerOf = new List<Dossier>() {dossierMock.Object}
            };
            _treatmentPlanRepository.Setup(c => c.Get(It.IsAny<int>())).ReturnsAsync(() =>
            {
                return new TreatmentPlan()
                {
                    Id = 0,
                    CreatedAt = new DateTime(2025,01,20,9,0,0),
                    TreatmentsPerWeek = 5,
                    TimePerSessionInMinutes = 50
                };
            });

            Appointment treatment = new Appointment()
            {
                Id = 0,
                Dossier = new Dossier()
                {
                    RegistrationDate = new DateTime(2025,01,20,9,0,0).AddDays(-2),
                    DismissionDate = new DateTime(2025,01,20,9,0,0).AddDays(3),
                    HeadPractitioner = staff,
                    TreatmentPlan = new TreatmentPlan()
                    {
                        Id = 1,
                        CreatedAt = new DateTime(2025,01,20,9,0,0),
                        TreatmentsPerWeek = 5,
                        TimePerSessionInMinutes = 50
                    },
                },
                Room = RoomType.None,
                CreatedAt = new DateTime(2025,01,20,9,0,0),
                ExcecutedBy = staff,
                TreatmentDate = new DateTime(2025,01,20,9,0,0).AddMinutes(5),
                TreatmentEndDate = new DateTime(2025,01,20,9,0,0).AddDays(1),
            };

            // Act
            var result = await sut.Add(treatment);

            // Assert
            Assert.Equal(result.Id, treatment.Id);
        }

        [Fact]
        public async Task AppointmentService_Create_must_not_error_when_Inside_WOrkHours()
        {
            // Arrange
            AppointmentService sut = new AppointmentService(repository.Object, _dossierService.Object,
                _treatmentPlanRepository.Object, _datetimeHelper.Object);
            Mock<Dossier> dossierMock = new Mock<Dossier>();
            dossierMock.Setup(d => d.TreatmentPlan).Returns(new TreatmentPlan()
            {
                Id = 1,
                CreatedAt = new DateTime(2025,01,20,9,0,0),
                TreatmentsPerWeek = 5,
                TimePerSessionInMinutes = 50
            });
            Staff staff = new Staff()
            {
                start = new TimeSpan(0, 0, 0),
                end = new TimeSpan(24, 0, 0),
                Id = 0,
                Email = "email",
                CreatedAt = new DateTime(2025,01,20,9,0,0),
                FirstName = "",
                CommentsCreated = new List<Comment>(),
                IntakesDone = new List<Dossier>(),
                IntakesSupervised = new List<Dossier>(),
                LastName = "",
                HeadPractisionerOf = new List<Dossier>() {dossierMock.Object}
            };
            _treatmentPlanRepository.Setup(c => c.Get(It.IsAny<int>())).ReturnsAsync(() =>
            {
                return new TreatmentPlan()
                {
                    Id = 0,
                    CreatedAt = new DateTime(2025,01,20,9,0,0),
                    TreatmentsPerWeek = 1,
                    TimePerSessionInMinutes = 50
                };
            });

            Appointment treatment = new Appointment()
            {
                Id = 0,
                Dossier = new Dossier()
                {
                    RegistrationDate = new DateTime(2025,01,20,9,0,0).AddDays(-2),
                    DismissionDate = new DateTime(2025,01,20,9,0,0).AddDays(3),
                    HeadPractitioner = staff,
                    TreatmentPlan = new TreatmentPlan()
                    {
                        Id = 1,
                        CreatedAt = new DateTime(2025,01,20,9,0,0),
                        TreatmentsPerWeek = 5,
                        TimePerSessionInMinutes = 50
                    },
                },
                Room = RoomType.None,
                CreatedAt = new DateTime(2025,01,20,9,0,0),
                ExcecutedBy = staff,
                TreatmentDate = new DateTime(2025,01,20,9,0,0).AddMinutes(5),
                TreatmentEndDate = new DateTime(2025,01,20,9,0,0).AddDays(1),
            };
            // Act
            var result = await sut.Add(treatment);

            // Assert
            Assert.Equal(result.Id, treatment.Id);
        }

        [Fact]
        public async Task AppointmentService_Create_must_Not_error_when_other_appointment_isnt_planned()
        {
            // Arrange
            AppointmentService sut = new AppointmentService(repository.Object, _dossierService.Object,
                _treatmentPlanRepository.Object, _datetimeHelper.Object);
            Mock<Dossier> dossierMock = new Mock<Dossier>();
            dossierMock.Setup(d => d.TreatmentPlan).Returns(new TreatmentPlan()
            {
                Id = 1,
                CreatedAt = new DateTime(2025,01,20,9,0,0),
                TreatmentsPerWeek = 5,
                TimePerSessionInMinutes = 50
            });
            Staff staff = new Staff()
            {
                start = new TimeSpan(0, 0, 0),
                end = new TimeSpan(24, 0, 0),
                Id = 0,
                Email = "email",
                CreatedAt = new DateTime(2025,01,20,9,0,0),
                FirstName = "",
                CommentsCreated = new List<Comment>(),
                IntakesDone = new List<Dossier>(),
                IntakesSupervised = new List<Dossier>(),
                LastName = "",
                HeadPractisionerOf = new List<Dossier>() {dossierMock.Object}
            };
            _treatmentPlanRepository.Setup(c => c.Get(It.IsAny<int>())).ReturnsAsync(() =>
            {
                return new TreatmentPlan()
                {
                    Id = 0,
                    CreatedAt = new DateTime(2025,01,20,9,0,0),
                    TreatmentsPerWeek = 1,
                    TimePerSessionInMinutes = 50
                };
            });
            dossierMock.Setup(d => d.Appointments).Returns(() =>
            {
                return new List<Appointment>()
                {
                };
            });


            Appointment treatment = new Appointment()
            {
                Id = 0,
                Dossier = new Dossier()
                {
                    RegistrationDate = new DateTime(2025,01,20,9,0,0).AddDays(-2),
                    DismissionDate = new DateTime(2025,01,20,9,0,0).AddDays(3),
                    HeadPractitioner = staff,
                    TreatmentPlan = new TreatmentPlan()
                    {
                        Id = 1,
                        CreatedAt = new DateTime(2025,01,20,9,0,0),
                        TreatmentsPerWeek = 5,
                        TimePerSessionInMinutes = 50
                    },
                },
                Room = RoomType.None,
                CreatedAt = new DateTime(2025,01,20,9,0,0),
                ExcecutedBy = staff,
                TreatmentDate = new DateTime(2025,01,20,9,0,0).AddMinutes(5),
                TreatmentEndDate = new DateTime(2025,01,20,9,0,0).AddHours(1),
            };
            // Act
            var result = await sut.Add(treatment);

            // Assert
            Assert.Equal(result.Id, treatment.Id);
        }


        [Fact]
        public async Task
            AppointmentService_Create_must_not_error_when_appointment_is_not_planned_outside_treatment_period()
        {
            // Arrange
            AppointmentService sut = new AppointmentService(repository.Object, _dossierService.Object,
                _treatmentPlanRepository.Object, _datetimeHelper.Object);
            Mock<Dossier> dossierMock = new Mock<Dossier>();
            dossierMock.Setup(d => d.TreatmentPlan).Returns(new TreatmentPlan()
            {
                Id = 1,
                CreatedAt = new DateTime(2025,01,20,9,0,0),
                TreatmentsPerWeek = 5,
                TimePerSessionInMinutes = 50
            });

            Staff staff = new Staff()
            {
                start = new TimeSpan(0, 0, 0),
                end = new TimeSpan(24, 0, 0),
                Id = 0,
                Email = "email",
                CreatedAt = new DateTime(2025,01,20,9,0,0),
                FirstName = "",
                CommentsCreated = new List<Comment>(),
                IntakesDone = new List<Dossier>(),
                IntakesSupervised = new List<Dossier>(),
                LastName = "",
                HeadPractisionerOf = new List<Dossier>() {dossierMock.Object}
            };
            _treatmentPlanRepository.Setup(c => c.Get(It.IsAny<int>())).ReturnsAsync(() =>
            {
                return new TreatmentPlan()
                {
                    Id = 0,
                    CreatedAt = new DateTime(2025,01,20,9,0,0),
                    TreatmentsPerWeek = 1,
                    TimePerSessionInMinutes = 50
                };
            });


            Appointment treatment = new Appointment()
            {
                Id = 0,
                Dossier = new Dossier()
                {
                    RegistrationDate = new DateTime(2025,01,20,9,0,0).AddDays(-2),
                    DismissionDate = new DateTime(2025,01,20,9,0,0).AddDays(3),
                    HeadPractitioner = staff,
                    TreatmentPlan = new TreatmentPlan()
                    {
                        Id = 1,
                        CreatedAt = new DateTime(2025,01,20,9,0,0),
                        TreatmentsPerWeek = 5,
                        TimePerSessionInMinutes = 50
                    },
                },
                Room = RoomType.None,
                CreatedAt = new DateTime(2025,01,20,9,0,0),
                ExcecutedBy = staff,
                TreatmentDate = new DateTime(2025,01,20,9,0,0).AddMinutes(5),
                TreatmentEndDate = new DateTime(2025,01,20,9,0,0).AddHours(1),
            };
            // Act
            var result = await sut.Add(treatment);

            // Assert
            Assert.Equal(result.Id, treatment.Id);
        }

        [Fact]
        public async Task AppointmentService_remove_must_not_error_when_called_More_than_24H_before_start()
        {
            // Arrange
            AppointmentService sut = new AppointmentService(repository.Object, _dossierService.Object,
                _treatmentPlanRepository.Object, _datetimeHelper.Object);
            Mock<Dossier> dossierMock = new Mock<Dossier>();
            dossierMock.Setup(d => d.TreatmentPlan).Returns(new TreatmentPlan()
            {
                Id = 1,
                CreatedAt = new DateTime(2025,01,20,9,0,0),
                TreatmentsPerWeek = 5,
                TimePerSessionInMinutes = 50
            });

            Staff staff = new Staff()
            {
                start = new TimeSpan(0, 0, 0),
                end = new TimeSpan(24, 0, 0),
                Id = 0,
                Email = "email",
                CreatedAt = new DateTime(2025,01,20,9,0,0),
                FirstName = "",
                CommentsCreated = new List<Comment>(),
                IntakesDone = new List<Dossier>(),
                IntakesSupervised = new List<Dossier>(),
                LastName = "",
                HeadPractisionerOf = new List<Dossier>() {dossierMock.Object}
            };
            _treatmentPlanRepository.Setup(c => c.Get(It.IsAny<int>())).ReturnsAsync(() =>
            {
                return new TreatmentPlan()
                {
                    Id = 0,
                    CreatedAt = new DateTime(2025,01,20,9,0,0),
                    TreatmentsPerWeek = 1,
                    TimePerSessionInMinutes = 50
                };
            });


            Appointment treatment = new Appointment()
            {
                Id = 0,
                Dossier = new Dossier()
                {
                    RegistrationDate = new DateTime(2025,01,20,9,0,0).AddDays(2),
                    DismissionDate = new DateTime(2025,01,20,9,0,0).AddDays(3),
                    HeadPractitioner = staff,
                    TreatmentPlan = new TreatmentPlan()
                    {
                        Id = 1,
                        CreatedAt = new DateTime(2025,01,20,9,0,0),
                        TreatmentsPerWeek = 5,
                        TimePerSessionInMinutes = 50
                    },
                },
                Room = RoomType.None,
                CreatedAt = new DateTime(2025,01,20,9,0,0),
                ExcecutedBy = staff,
                TreatmentDate = new DateTime(2025,01,20,9,0,0).AddHours(25),
                TreatmentEndDate = new DateTime(2025,01,20,9,0,0).AddHours(26),
            };
            // Act
            await sut.Delete(treatment);

            // Assert
            repository.Verify(r => r.Delete(It.IsAny<Appointment>()), Times.Once);
        }

        [Fact]
        public async Task AppointmentService_Update_must_error_when_Max_Amount_Of_Treatments_In_Week_Is_Surpassed()
        {
            // Arrange
            AppointmentService sut = new AppointmentService(repository.Object, _dossierService.Object,
                _treatmentPlanRepository.Object, _datetimeHelper.Object);
            Mock<Dossier> dossierMock = new Mock<Dossier>();
            dossierMock.Setup(d => d.TreatmentPlan).Returns(new TreatmentPlan()
            {
                Id = 1,
                CreatedAt = new DateTime(2025,01,20,9,0,0),
                TreatmentsPerWeek = 5,
                TimePerSessionInMinutes = 50
            });
            Staff staff = new Staff()
            {
                start = new TimeSpan(0, 0, 0),
                end = new TimeSpan(24, 0, 0),
                Id = 0,
                Email = "email",
                CreatedAt = new DateTime(2025,01,20,9,0,0),
                FirstName = "",
                CommentsCreated = new List<Comment>(),
                IntakesDone = new List<Dossier>(),
                IntakesSupervised = new List<Dossier>(),
                LastName = "",
                HeadPractisionerOf = new List<Dossier>() {dossierMock.Object}
            };
            _treatmentPlanRepository.Setup(c => c.Get(It.IsAny<int>())).ReturnsAsync(() =>
            {
                return new TreatmentPlan()
                {
                    Id = 0,
                    CreatedAt = new DateTime(2025,01,20,9,0,0),
                    TreatmentsPerWeek = 0,
                    TimePerSessionInMinutes = 50
                };
            });

            Appointment treatment = new Appointment()
            {
                Id = 0,
                Dossier = dossierMock.Object,
                Room = RoomType.None,
                CreatedAt = new DateTime(2025,01,20,9,0,0),
                ExcecutedBy = staff,
                TreatmentDate = new DateTime(2025,01,20,9,0,0).AddMinutes(5),
                TreatmentEndDate = new DateTime(2025,01,20,9,0,0).AddDays(1),
            };

            // Act
            Func<Task> act = () => sut.Update(treatment);

            // Assert
            ValidationException exception = await Assert.ThrowsAsync<ValidationException>(act);

            Assert.Equal("Het maximum aantal afspraken zijn al aangemaakt voor deze week", exception.Message);
        }

        [Fact]
        public async Task AppointmentService_Update_must_error_when_OutsideWOrkHours()
        {
            // Arrange
            AppointmentService sut = new AppointmentService(repository.Object, _dossierService.Object,
                _treatmentPlanRepository.Object, _datetimeHelper.Object);
            Mock<Dossier> dossierMock = new Mock<Dossier>();
            dossierMock.Setup(d => d.TreatmentPlan).Returns(new TreatmentPlan()
            {
                Id = 1,
                CreatedAt = new DateTime(2025,01,20,9,0,0),
                TreatmentsPerWeek = 5,
                TimePerSessionInMinutes = 50
            });
            Staff staff = new Staff()
            {
                start = new TimeSpan(0, 0, 0),
                end = new TimeSpan(0, 0, 0),
                Id = 0,
                Email = "email",
                CreatedAt = new DateTime(2025,01,20,9,0,0),
                FirstName = "",
                CommentsCreated = new List<Comment>(),
                IntakesDone = new List<Dossier>(),
                IntakesSupervised = new List<Dossier>(),
                LastName = "",
                HeadPractisionerOf = new List<Dossier>() {dossierMock.Object}
            };
            _treatmentPlanRepository.Setup(c => c.Get(It.IsAny<int>())).ReturnsAsync(() =>
            {
                return new TreatmentPlan()
                {
                    Id = 0,
                    CreatedAt = new DateTime(2025,01,20,9,0,0),
                    TreatmentsPerWeek = 1,
                    TimePerSessionInMinutes = 50
                };
            });

            Appointment treatment = new Appointment()
            {
                Id = 0,
                Dossier = dossierMock.Object,
                Room = RoomType.None,
                CreatedAt = new DateTime(2025,01,20,9,0,0),
                ExcecutedBy = staff,
                TreatmentDate = new DateTime(2025,01,20,9,0,0).AddMinutes(5),
                TreatmentEndDate = new DateTime(2025,01,20,9,0,0).AddDays(1),
            };
            // Act
            Func<Task> act = () => sut.Update(treatment);

            // Assert
            ValidationException exception = await Assert.ThrowsAsync<ValidationException>(act);

            Assert.Equal("Deze behandelinmg valt buiten de werktijden van uw doctor", exception.Message);
        }

        [Fact]
        public async Task AppointmentService_Update_must_error_when_other_appointment_is_planned()
        {
            // Arrange
            AppointmentService sut = new AppointmentService(repository.Object, _dossierService.Object,
                _treatmentPlanRepository.Object, _datetimeHelper.Object);
            Mock<Dossier> dossierMock = new Mock<Dossier>();
            dossierMock.Setup(d => d.TreatmentPlan).Returns(new TreatmentPlan()
            {
                Id = 1,
                CreatedAt = new DateTime(2025,01,20,9,0,0),
                TreatmentsPerWeek = 5,
                TimePerSessionInMinutes = 50
            });
            Staff staff = new Staff()
            {
                start = new TimeSpan(0, 0, 0),
                end = new TimeSpan(24, 0, 0),
                Id = 0,
                Email = "email",
                CreatedAt = new DateTime(2025,01,20,9,0,0),
                FirstName = "",
                CommentsCreated = new List<Comment>(),
                IntakesDone = new List<Dossier>(),
                IntakesSupervised = new List<Dossier>(),
                LastName = "",
                HeadPractisionerOf = new List<Dossier>() {dossierMock.Object}
            };
            _treatmentPlanRepository.Setup(c => c.Get(It.IsAny<int>())).ReturnsAsync(() =>
            {
                return new TreatmentPlan()
                {
                    Id = 0,
                    CreatedAt = new DateTime(2025,01,20,9,0,0),
                    TreatmentsPerWeek = 1,
                    TimePerSessionInMinutes = 50
                };
            });
            dossierMock.Setup(d => d.Appointments).Returns(() =>
            {
                return new List<Appointment>()
                {
                    new Appointment()
                    {
                        Id = 0,
                        Dossier = dossierMock.Object,
                        Room = RoomType.None,
                        CreatedAt = new DateTime(2025,01,20,9,0,0),
                        ExcecutedBy = staff,
                        TreatmentDate = new DateTime(2025,01,20,9,0,0),
                        TreatmentEndDate = new DateTime(2025,01,20,9,0,0).AddHours(2),
                    }
                };
            });


            Appointment treatment = new Appointment()
            {
                Id = 0,
                Dossier = dossierMock.Object,
                Room = RoomType.None,
                CreatedAt = new DateTime(2025,01,20,9,0,0),
                ExcecutedBy = staff,
                TreatmentDate = new DateTime(2025,01,20,9,0,0).AddMinutes(5),
                TreatmentEndDate = new DateTime(2025,01,20,9,0,0).AddHours(1),
            };
            // Act
            Func<Task> act = () => sut.Update(treatment);

            // Assert
            ValidationException exception = await Assert.ThrowsAsync<ValidationException>(act);

            Assert.Equal("Uw doctor is al bezet op dit moment", exception.Message);
        }

        [Fact]
        public async Task AppointmentService_Update_must_error_when_appointment_is_planned_outside_treatment_period()
        {
            // Arrange
            AppointmentService sut = new AppointmentService(repository.Object, _dossierService.Object,
                _treatmentPlanRepository.Object, _datetimeHelper.Object);
            Mock<Dossier> dossierMock = new Mock<Dossier>();
            dossierMock.Setup(d => d.TreatmentPlan).Returns(new TreatmentPlan()
            {
                Id = 1,
                CreatedAt = new DateTime(2025,01,20,9,0,0),
                TreatmentsPerWeek = 5,
                TimePerSessionInMinutes = 50
            });

            Staff staff = new Staff()
            {
                start = new TimeSpan(0, 0, 0),
                end = new TimeSpan(24, 0, 0),
                Id = 0,
                Email = "email",
                CreatedAt = new DateTime(2025,01,20,9,0,0),
                FirstName = "",
                CommentsCreated = new List<Comment>(),
                IntakesDone = new List<Dossier>(),
                IntakesSupervised = new List<Dossier>(),
                LastName = "",
                HeadPractisionerOf = new List<Dossier>() {dossierMock.Object}
            };
            _treatmentPlanRepository.Setup(c => c.Get(It.IsAny<int>())).ReturnsAsync(() =>
            {
                return new TreatmentPlan()
                {
                    Id = 0,
                    CreatedAt = new DateTime(2025,01,20,9,0,0),
                    TreatmentsPerWeek = 1,
                    TimePerSessionInMinutes = 50
                };
            });


            Appointment treatment = new Appointment()
            {
                Id = 0,
                Dossier = new Dossier()
                {
                    RegistrationDate = new DateTime(2025,01,20,9,0,0).AddDays(2),
                    DismissionDate = new DateTime(2025,01,20,9,0,0).AddDays(3),
                    HeadPractitioner = staff,
                    TreatmentPlan = new TreatmentPlan()
                    {
                        Id = 1,
                        CreatedAt = new DateTime(2025,01,20,9,0,0),
                        TreatmentsPerWeek = 5,
                        TimePerSessionInMinutes = 50
                    },
                },
                Room = RoomType.None,
                CreatedAt = new DateTime(2025,01,20,9,0,0),
                ExcecutedBy = staff,
                TreatmentDate = new DateTime(2025,01,20,9,0,0).AddMinutes(5),
                TreatmentEndDate = new DateTime(2025,01,20,9,0,0).AddHours(1),
            };
            // Act
            Func<Task> act = () => sut.Update(treatment);

            // Assert
            ValidationException exception = await Assert.ThrowsAsync<ValidationException>(act);

            Assert.Equal("een behandeling kan niet geplanned worden buiten een behandel periode", exception.Message);
        }

        [Fact]
        public async Task
            AppointmentService_Update_must_not_error_when_Max_Amount_Of_Treatments_In_Week_Is_Not_Surpassed()
        {
            // Arrange
            AppointmentService sut = new AppointmentService(repository.Object, _dossierService.Object,
                _treatmentPlanRepository.Object, _datetimeHelper.Object);
            Mock<Dossier> dossierMock = new Mock<Dossier>();
            dossierMock.Setup(d => d.TreatmentPlan).Returns(new TreatmentPlan()
            {
                Id = 1,
                CreatedAt = new DateTime(2025,01,20,9,0,0),
                TreatmentsPerWeek = 5,
                TimePerSessionInMinutes = 50
            });
            Staff staff = new Staff()
            {
                start = new TimeSpan(0, 0, 0),
                end = new TimeSpan(24, 0, 0),
                Id = 0,
                Email = "email",
                CreatedAt = new DateTime(2025,01,20,9,0,0),
                FirstName = "",
                CommentsCreated = new List<Comment>(),
                IntakesDone = new List<Dossier>(),
                IntakesSupervised = new List<Dossier>(),
                LastName = "",
                HeadPractisionerOf = new List<Dossier>() {dossierMock.Object}
            };
            _treatmentPlanRepository.Setup(c => c.Get(It.IsAny<int>())).ReturnsAsync(() =>
            {
                return new TreatmentPlan()
                {
                    Id = 0,
                    CreatedAt = new DateTime(2025,01,20,9,0,0),
                    TreatmentsPerWeek = 1,
                    TimePerSessionInMinutes = 50
                };
            });

            Appointment treatment = new Appointment()
            {
                Id = 0,
                Dossier = new Dossier()
                {
                    RegistrationDate = new DateTime(2025,01,20,9,0,0).AddDays(-2),
                    DismissionDate = new DateTime(2025,01,20,9,0,0).AddDays(3),
                    HeadPractitioner = staff,
                    TreatmentPlan = new TreatmentPlan()
                    {
                        Id = 1,
                        CreatedAt = new DateTime(2025,01,20,9,0,0),
                        TreatmentsPerWeek = 5,
                        TimePerSessionInMinutes = 50
                    },
                },
                Room = RoomType.None,
                CreatedAt = new DateTime(2025,01,20,9,0,0),
                ExcecutedBy = staff,
                TreatmentDate = new DateTime(2025,01,20,9,0,0).AddMinutes(5),
                TreatmentEndDate = new DateTime(2025,01,20,9,0,0).AddDays(1),
            };
            // Act
            var result = await sut.Update(treatment);

            // Assert
            Assert.Equal(result.Id, treatment.Id);
        }

        [Fact]
        public async Task
            AppointmentService_Update_must_Not_error_when_Max_Amount_Of_Treatments_In_Week_Is_not_Surpassed()
        {
            // Arrange
            AppointmentService sut = new AppointmentService(repository.Object, _dossierService.Object,
                _treatmentPlanRepository.Object, _datetimeHelper.Object);
            Mock<Dossier> dossierMock = new Mock<Dossier>();
            dossierMock.Setup(d => d.TreatmentPlan).Returns(new TreatmentPlan()
            {
                Id = 1,
                CreatedAt = new DateTime(2025,01,20,9,0,0),
                TreatmentsPerWeek = 5,
                TimePerSessionInMinutes = 50
            });
            Staff staff = new Staff()
            {
                start = new TimeSpan(0, 0, 0),
                end = new TimeSpan(24, 0, 0),
                Id = 0,
                Email = "email",
                CreatedAt = new DateTime(2025,01,20,9,0,0),
                FirstName = "",
                CommentsCreated = new List<Comment>(),
                IntakesDone = new List<Dossier>(),
                IntakesSupervised = new List<Dossier>(),
                LastName = "",
                HeadPractisionerOf = new List<Dossier>() {dossierMock.Object}
            };
            _treatmentPlanRepository.Setup(c => c.Get(It.IsAny<int>())).ReturnsAsync(() =>
            {
                return new TreatmentPlan()
                {
                    Id = 0,
                    CreatedAt = new DateTime(2025,01,20,9,0,0),
                    TreatmentsPerWeek = 5,
                    TimePerSessionInMinutes = 50
                };
            });

            Appointment treatment = new Appointment()
            {
                Id = 0,
                Dossier = new Dossier()
                {
                    RegistrationDate = new DateTime(2025,01,20,9,0,0).AddDays(-2),
                    DismissionDate = new DateTime(2025,01,20,9,0,0).AddDays(3),
                    HeadPractitioner = staff,
                    TreatmentPlan = new TreatmentPlan()
                    {
                        Id = 1,
                        CreatedAt = new DateTime(2025,01,20,9,0,0),
                        TreatmentsPerWeek = 5,
                        TimePerSessionInMinutes = 50
                    },
                },
                Room = RoomType.None,
                CreatedAt = new DateTime(2025,01,20,9,0,0),
                ExcecutedBy = staff,
                TreatmentDate = new DateTime(2025,01,20,9,0,0).AddMinutes(5),
                TreatmentEndDate = new DateTime(2025,01,20,9,0,0).AddDays(1),
            };

            // Act
            var result = await sut.Update(treatment);

            // Assert
            Assert.Equal(result.Id, treatment.Id);
        }

        [Fact]
        public async Task AppointmentService_Update_must_not_error_when_Inside_WOrkHours()
        {
            // Arrange
            AppointmentService sut = new AppointmentService(repository.Object, _dossierService.Object,
                _treatmentPlanRepository.Object, _datetimeHelper.Object);
            Mock<Dossier> dossierMock = new Mock<Dossier>();
            dossierMock.Setup(d => d.TreatmentPlan).Returns(new TreatmentPlan()
            {
                Id = 1,
                CreatedAt = new DateTime(2025,01,20,9,0,0),
                TreatmentsPerWeek = 5,
                TimePerSessionInMinutes = 50
            });
            Staff staff = new Staff()
            {
                start = new TimeSpan(0, 0, 0),
                end = new TimeSpan(24, 0, 0),
                Id = 0,
                Email = "email",
                CreatedAt = new DateTime(2025,01,20,9,0,0),
                FirstName = "",
                CommentsCreated = new List<Comment>(),
                IntakesDone = new List<Dossier>(),
                IntakesSupervised = new List<Dossier>(),
                LastName = "",
                HeadPractisionerOf = new List<Dossier>() {dossierMock.Object}
            };
            _treatmentPlanRepository.Setup(c => c.Get(It.IsAny<int>())).ReturnsAsync(() =>
            {
                return new TreatmentPlan()
                {
                    Id = 0,
                    CreatedAt = new DateTime(2025,01,20,9,0,0),
                    TreatmentsPerWeek = 1,
                    TimePerSessionInMinutes = 50
                };
            });

            Appointment treatment = new Appointment()
            {
                Id = 0,
                Dossier = new Dossier()
                {
                    RegistrationDate = new DateTime(2025,01,20,9,0,0).AddDays(-2),
                    DismissionDate = new DateTime(2025,01,20,9,0,0).AddDays(3),
                    HeadPractitioner = staff,
                    TreatmentPlan = new TreatmentPlan()
                    {
                        Id = 1,
                        CreatedAt = new DateTime(2025,01,20,9,0,0),
                        TreatmentsPerWeek = 5,
                        TimePerSessionInMinutes = 50
                    },
                },
                Room = RoomType.None,
                CreatedAt = new DateTime(2025,01,20,9,0,0),
                ExcecutedBy = staff,
                TreatmentDate = new DateTime(2025,01,20,9,0,0).AddMinutes(5),
                TreatmentEndDate = new DateTime(2025,01,20,9,0,0).AddDays(1),
            };
            // Act
            var result = await sut.Update(treatment);

            // Assert
            Assert.Equal(result.Id, treatment.Id);
        }

        [Fact]
        public async Task AppointmentService_Update_must_Not_error_when_other_appointment_isnt_planned()
        {
            // Arrange
            AppointmentService sut = new AppointmentService(repository.Object, _dossierService.Object,
                _treatmentPlanRepository.Object, _datetimeHelper.Object);
            Mock<Dossier> dossierMock = new Mock<Dossier>();
            dossierMock.Setup(d => d.TreatmentPlan).Returns(new TreatmentPlan()
            {
                Id = 1,
                CreatedAt = new DateTime(2025,01,20,9,0,0),
                TreatmentsPerWeek = 5,
                TimePerSessionInMinutes = 50
            });
            Staff staff = new Staff()
            {
                start = new TimeSpan(0, 0, 0),
                end = new TimeSpan(24, 0, 0),
                Id = 0,
                Email = "email",
                CreatedAt = new DateTime(2025,01,20,9,0,0),
                FirstName = "",
                CommentsCreated = new List<Comment>(),
                IntakesDone = new List<Dossier>(),
                IntakesSupervised = new List<Dossier>(),
                LastName = "",
                HeadPractisionerOf = new List<Dossier>() {dossierMock.Object}
            };
            _treatmentPlanRepository.Setup(c => c.Get(It.IsAny<int>())).ReturnsAsync(() =>
            {
                return new TreatmentPlan()
                {
                    Id = 0,
                    CreatedAt = new DateTime(2025,01,20,9,0,0),
                    TreatmentsPerWeek = 1,
                    TimePerSessionInMinutes = 50
                };
            });
            dossierMock.Setup(d => d.Appointments).Returns(() =>
            {
                return new List<Appointment>()
                {
                };
            });


            Appointment treatment = new Appointment()
            {
                Id = 0,
                Dossier = new Dossier()
                {
                    RegistrationDate = new DateTime(2025,01,20,9,0,0).AddDays(-2),
                    DismissionDate = new DateTime(2025,01,20,9,0,0).AddDays(3),
                    HeadPractitioner = staff,
                    TreatmentPlan = new TreatmentPlan()
                    {
                        Id = 1,
                        CreatedAt = new DateTime(2025,01,20,9,0,0),
                        TreatmentsPerWeek = 5,
                        TimePerSessionInMinutes = 50
                    },
                },
                Room = RoomType.None,
                CreatedAt = new DateTime(2025,01,20,9,0,0),
                ExcecutedBy = staff,
                TreatmentDate = new DateTime(2025,01,20,9,0,0).AddMinutes(5),
                TreatmentEndDate = new DateTime(2025,01,20,9,0,0).AddHours(1),
            };
            // Act
            var result = await sut.Update(treatment);

            // Assert
            Assert.Equal(result.Id, treatment.Id);
        }


        [Fact]
        public async Task
            AppointmentService_Update_must_not_error_when_appointment_is_not_planned_outside_treatment_period()
        {
            // Arrange
            AppointmentService sut = new AppointmentService(repository.Object, _dossierService.Object,
                _treatmentPlanRepository.Object, _datetimeHelper.Object);
            Mock<Dossier> dossierMock = new Mock<Dossier>();
            dossierMock.Setup(d => d.TreatmentPlan).Returns(new TreatmentPlan()
            {
                Id = 1,
                CreatedAt = new DateTime(2025,01,20,9,0,0),
                TreatmentsPerWeek = 5,
                TimePerSessionInMinutes = 50
            });

            Staff staff = new Staff()
            {
                start = new TimeSpan(0, 0, 0),
                end = new TimeSpan(24, 0, 0),
                Id = 0,
                Email = "email",
                CreatedAt = new DateTime(2025,01,20,9,0,0),
                FirstName = "",
                CommentsCreated = new List<Comment>(),
                IntakesDone = new List<Dossier>(),
                IntakesSupervised = new List<Dossier>(),
                LastName = "",
                HeadPractisionerOf = new List<Dossier>() {dossierMock.Object}
            };
            _treatmentPlanRepository.Setup(c => c.Get(It.IsAny<int>())).ReturnsAsync(() =>
            {
                return new TreatmentPlan()
                {
                    Id = 0,
                    CreatedAt = new DateTime(2025,01,20,9,0,0),
                    TreatmentsPerWeek = 1,
                    TimePerSessionInMinutes = 50
                };
            });


            Appointment treatment = new Appointment()
            {
                Id = 0,
                Dossier = new Dossier()
                {
                    RegistrationDate = new DateTime(2025,01,20,9,0,0).AddDays(-2),
                    DismissionDate = new DateTime(2025,01,20,9,0,0).AddDays(3),
                    HeadPractitioner = staff,
                    TreatmentPlan = new TreatmentPlan()
                    {
                        Id = 1,
                        CreatedAt = new DateTime(2025,01,20,9,0,0),
                        TreatmentsPerWeek = 5,
                        TimePerSessionInMinutes = 50
                    },
                },
                Room = RoomType.None,
                CreatedAt = new DateTime(2025,01,20,9,0,0),
                ExcecutedBy = staff,
                TreatmentDate = new DateTime(2025,01,20,9,0,0).AddMinutes(5),
                TreatmentEndDate = new DateTime(2025,01,20,9,0,0).AddHours(1),
            };
            // Act
            var result = await sut.Update(treatment);

            // Assert
            Assert.Equal(result.Id, treatment.Id);
        }
        
    }
}