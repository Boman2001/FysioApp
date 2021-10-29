using AutoMapper;
using Core.Domain.Models;
using Core.DomainServices.Interfaces;
using StamApi.Models.Domain;

namespace StamApi.Controllers
{
    public class DiagnoseCodesController : Controller<DiagnoseCode, DiagnoseCodeDto>
    {
        public DiagnoseCodesController(IRepository<DiagnoseCode> repository, IIdentityRepository identityRepository, IMapper mapper) : base(repository, identityRepository, mapper)
        {
        }
        
        
        
    }
}