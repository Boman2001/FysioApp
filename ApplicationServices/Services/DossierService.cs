using System;
using System.Threading.Tasks;
using Core.Domain.Models;
using Core.DomainServices.Interfaces;

namespace ApplicationServices.Services
{
    public class DossierService : Service<Dossier>
    {
        public DossierService(IRepository<Dossier> repository) : base(repository)
        {
        }

        public Task<Dossier> Add(Dossier model)
        {
            if (model.IntakeBy.GetType() == typeof(Student))
            {
                
            }
            return _repository.Add(model);
        }
    }
}