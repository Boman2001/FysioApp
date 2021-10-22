using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Domain.Models;
using Core.DomainServices.Interfaces;

namespace ApplicationServices.Services
{
    public class PatientService : Service<Patient>
    {
        public PatientService(IRepository<Patient> repository) : base(repository)
        {
        }
        
        public Task<Patient> Add(Patient model)
        {
            DateTime today = DateTime.Now;
            if (model.BirthDay.Year - today.Year >= 16 )
            {
                return _repository.Add(model);
            }
            else
            {
                throw new Exception("Could not create Patient Reason: He is to young");
            }

        }
    }
}