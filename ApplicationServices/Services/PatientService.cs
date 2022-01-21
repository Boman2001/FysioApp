using System;
using System.Linq;
using System.Threading.Tasks;
using Core.Domain.Exceptions;
using Core.Domain.Models;
using Core.DomainServices.Interfaces;

namespace ApplicationServices.Services
{
    public class PatientService : Service<Patient>, IService<Patient>
    {
        public PatientService(IRepository<Patient> repository) : base(repository)
        {
        }

        public new async Task<Patient> Add(Patient model)
        {
            DateTime today = DateTime.Now;
            if (today.Year - model.BirthDay.Year < 16)
            {
                throw new ValidationException("Patient is not old enough must be atleast 16");
            }

            if (_repository.Get( a => a.Email == model.Email).Count() > 0)
            {
                throw new ValidationException("Patient email not unique");
            }

            return await _repository.Add(model);
        }

        public new async Task<Patient> Update(int id,Patient model)
        {
            DateTime today = DateTime.Now;
            if (today.Year - model.BirthDay.Year >= 16)
            {
                return await _repository.Update(id, model);
            }
            else
            {
                throw new ValidationException("Patient is not old enough must be atleast 16");
            }
        }

        public new async Task<Patient> Update(Patient model)
        {
            DateTime today = DateTime.Now;
            if (today.Year - model.BirthDay.Year >= 16)
            {
                return await _repository.Update( model);
            }
            else
            {
                throw new ValidationException("Patient is not old enough must be atleast 16");
            }
        }
    }
}