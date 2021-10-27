using Core.Domain.Models;
using Core.DomainServices.Interfaces;

namespace ApplicationServices.Services
{
    public class CommentService : Service<Comment>, IService<Comment>
    {
        public CommentService(IRepository<Comment> repository) : base(repository)
        {
        }
    }
}