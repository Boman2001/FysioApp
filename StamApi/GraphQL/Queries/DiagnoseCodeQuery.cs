using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Domain.Models;
using Core.DomainServices.Interfaces;
using HotChocolate;
using HotChocolate.Types;
using StamApi.GraphQL.Root;

namespace StamApi.GraphQL.Queries
{
    [ExtendObjectType(typeof(Query))]
    public class DiagnoseCodeQuery {
        
        public IEnumerable<DiagnoseCode> diagnoseCodes([Service]IRepository<DiagnoseCode> diagnosisTypeApplicationService) {

            return diagnosisTypeApplicationService.Get();
        }
        
        public async Task<DiagnoseCode> diagnoseCode(int id, [Service]IRepository<DiagnoseCode> diagnosisTypeApplicationService) {

            return await diagnosisTypeApplicationService.Get(id);
        }
    }
}