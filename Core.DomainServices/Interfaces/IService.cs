﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Domain.Models;

namespace Core.DomainServices.Interfaces
{
    public interface IService<T> where T : Entity
    {
        IEnumerable<T> Get();
        Task<T> Get(int id);
        T Get(int id, IEnumerable<string> includeProperties);
        IEnumerable<T> Get(Expression<Func<T, bool>> filter);
        IEnumerable<T> Get(IEnumerable<string> includeProperties);
        IEnumerable<T> Get(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy);
        IEnumerable<T> Get(Expression<Func<T, bool>> filter, IEnumerable<string> includeProperties);

        IEnumerable<T> Get(Expression<Func<T, bool>> filter, IEnumerable<string> includeProperties,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy);
        Task<T> Add(T model);
        Task<T> Update(T model);
        Task<T> Update(int id, T model);
        Task Delete(int id);
        Task Delete(T model);
    }
}