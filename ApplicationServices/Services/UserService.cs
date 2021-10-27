using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Domain.Models;
using Core.DomainServices.Interfaces;

namespace ApplicationServices.Services
{
    public class UserService : Service<User> , IUserService
    {
        public UserService(IRepository<User> repository) : base(repository)
        {
        }

        public IEnumerable<User> GetDoctors()
        {
            return _repository.Get().Where(u => u is Doctor);
        }

        public IEnumerable<User> GetStudents()
        {
            return _repository.Get().Where(u => u is Student);
        }

        public IEnumerable<User> GetPatients()
        {
            return _repository.Get().Where(u => u is Patient);
        }

        public IEnumerable<User> GetStaff()
        {
            return _repository.Get().Where(u => u is Doctor or Student);
        }
    }
}