using System;
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
                throw new ValidationException("Patiënt is te jong");
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
                throw new ValidationException("Patiënt is te jong");
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
                throw new ValidationException("Patiënt is te jong");
            }
        }
    }
}