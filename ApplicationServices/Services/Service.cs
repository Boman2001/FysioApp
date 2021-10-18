using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Domain.Models;
using Core.DomainServices.Interfaces;

namespace ApplicationServices.Services
{
    public abstract class Service<T> :IService<T> where T : Entity
    {
        internal readonly IRepository<T> _repository;

        protected Service(IRepository<T> repository)
        {
            _repository = repository;
        }

        public IEnumerable<T> Get()
        {
            return _repository.Get();
        }

        public Task<T> Get(int id)
        {
            return _repository.Get(id);
        }

        public T Get(int id, IEnumerable<string> includeProperties)
        {
            return _repository.Get(id, includeProperties);
        }

        public IEnumerable<T> Get(Expression<Func<T, bool>> filter)
        {
            return _repository.Get(filter);
        }

        public IEnumerable<T> Get(IEnumerable<string> includeProperties)
        {
            return _repository.Get(includeProperties);
        }

        public IEnumerable<T> Get(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy)
        {
            return _repository.Get(orderBy);
        }

        public IEnumerable<T> Get(Expression<Func<T, bool>> filter, IEnumerable<string> includeProperties)
        {
           return _repository.Get(filter, includeProperties);
        }

        public IEnumerable<T> Get(Expression<Func<T, bool>> filter, IEnumerable<string> includeProperties, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy)
        {
            return _repository.Get(filter, includeProperties, orderBy);
        }

        public Task<T> Add(T model)
        {
            return _repository.Add(model);
        }

        public Task<T> Update(T model)
        {
            return _repository.Update(model);
        }

        public Task Delete(int id)
        {
            return _repository.Delete(id);
        }

        public Task Delete(T model)
        {
            return _repository.Delete(model);
        }
    }
}