using System;
using System.Threading.Tasks;
using Core.Domain.Exceptions;
using Core.Domain.Models;
using Core.DomainServices.Interfaces;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ApplicationServices.Services
{
    public class DossierService : Service<Dossier>, IService<Dossier>
    {
        private readonly IRepository<Student> _studentRepository;
        private readonly IRepository<Doctor> _doctorRepository;
        public DossierService(IRepository<Dossier> repository, IRepository<Student> studentRepository, IRepository<Doctor> doctorRepository) : base(repository)
        {
            _studentRepository = studentRepository;
            _doctorRepository = doctorRepository;
        }

        public DossierService(IRepository<Dossier> repository) : base(repository)
        {
        }

        
        public new Task<Dossier> Add(Dossier model)
        {
            model.Age = CalculateAgeCorrect(model.Patient.BirthDay, model.RegistrationDate);
            if (IsStudent(model.IntakeBy.Id) && (model.SupervisedBy is null || IsStudent(model.SupervisedBy.Id)))
            {
                throw new ValidationException("Een student mag alleen een intake uitvoeren oder supervisie van een doctor ");
            }

            if (model.DismissionDate.Ticks < model.RegistrationDate.Ticks)
            {
                throw new ValidationException("Onstlag datum moet na de registratie datum liggen");
            }

            return _repository.Add(model);
        }
        private int CalculateAgeCorrect(DateTime birthDate, DateTime now)
        {
            int age = now.Year - birthDate.Year;

            if (now.Month < birthDate.Month || (now.Month == birthDate.Month && now.Day < birthDate.Day))
                age--;

            return age;
        }

        private bool IsStudent(int id)
        {
            if (_doctorRepository.Get(id).Result == null && _studentRepository.Get(id).Result  != null)
            {
                return true;
            }

            return false;
        }
    }
}