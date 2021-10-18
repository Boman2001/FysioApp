using AutoMapper;
using Core.Domain.Models;
using Core.DomainServices.Interfaces;
using StamApi.Models.Domain;

namespace StamApi.Controllers
{
    public class DiagnoseCodeController : Controller<DiagnoseCode, DiagnoseCodeDto>
    {
        public DiagnoseCodeController(IRepository<DiagnoseCode> repository, IIdentityRepository identityRepository, IMapper mapper) : base(repository, identityRepository, mapper)
        {
        }
    }
}