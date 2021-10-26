using Core.Domain.Models;
using Core.DomainServices.Interfaces;

namespace ApplicationServices.Services
{
    public class TreatmentService : Service<Treatment>
    {
        public TreatmentService(IRepository<Treatment> repository) : base(repository)
        {
        }
    }
}