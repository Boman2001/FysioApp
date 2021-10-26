using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Domain.Models;
using Core.DomainServices.Interfaces;

namespace Infrastructure.API.Repositories
{
    public interface IWebRepository<T> :  IRepository<T> where T : Entity
    {
       Task<IEnumerable<T>> GetAsync();
       
       
    }
}