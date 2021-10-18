using Core.Domain.Models;
using Core.DomainServices.Interfaces;

namespace ApplicationServices.Services
{
    public class DossierService : Service<Dossier>
    {
        public DossierService(IRepository<Dossier> repository) : base(repository)
        {
        }
    }
}