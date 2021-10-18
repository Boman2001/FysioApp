using AutoMapper;
using Core.Domain.Models;
using Core.DomainServices.Interfaces;
using StamApi.Models.Domain;

namespace StamApi.Controllers
{
    public class TreatmentCodeController : Controller<TreatmentCode, TreatmentCodeDto>
    {
        public TreatmentCodeController(IRepository<TreatmentCode> repository, IIdentityRepository identityRepository, IMapper mapper) : base(repository, identityRepository, mapper)
        {
        }
    }
}