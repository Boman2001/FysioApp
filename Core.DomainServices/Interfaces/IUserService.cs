using System.Collections.Generic;
using Core.Domain.Models;

namespace Core.DomainServices.Interfaces
{
    public interface IUserService : IService<User>
    {
        IEnumerable<User> GetDoctors();
        IEnumerable<User> GetStudents();
        IEnumerable<User> GetPatients();
        IEnumerable<User> GetStaff();
    }
}