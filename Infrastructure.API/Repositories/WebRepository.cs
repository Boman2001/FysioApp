using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Domain.Models;
using Core.DomainServices.Interfaces;

namespace Infrastructure.API.Repositories
{
    public class WebRepository<T> : IRepository<T> where T : Entity
    {
        public IEnumerable<T> Get()
        {
            throw new NotImplementedException();
        }

        public Task<T> Get(int id)
        {
            throw new NotImplementedException();
        }

        public T Get(int id, IEnumerable<string> includeProperties)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> Get(Expression<Func<T, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> Get(IEnumerable<string> includeProperties)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> Get(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> Get(Expression<Func<T, bool>> filter, IEnumerable<string> includeProperties)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> Get(Expression<Func<T, bool>> filter, IEnumerable<string> includeProperties, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy)
        {
            throw new NotImplementedException();
        }

        public Task<T> Add(T model)
        {
            throw new NotImplementedException();
        }

        public Task<T> Update(T model)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task Delete(T model)
        {
            throw new NotImplementedException();
        }

        public Task Save()
        {
            throw new NotImplementedException();
        }

        public void Detach(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        public void Detach(T entity)
        {
            throw new NotImplementedException();
        }
    }
}