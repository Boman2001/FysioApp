using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Domain.Models;
using Core.DomainServices.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update.Internal;

namespace Core.Infrastructure.Repositories
{
    public class DatabaseRepository<T> : IRepository<T>
        where T : Entity
    {
        internal readonly DbContext _context;
        internal readonly DbSet<T> _dbSet;
        public DatabaseRepository(DbContext context)
        {
            _context = context; 
            _dbSet = _context.Set<T>();
        }

        public IEnumerable<T> Get()
        {
            return _dbSet.ToList();
        }

        public async Task<IEnumerable<T>> GetAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public async Task<T> Get(int id)
        {
            return await _dbSet.SingleOrDefaultAsync(e => e.Id == id);
        }

        public T Get(int id, IEnumerable<string> includeProperties)
        {
            return Get(e => e.Id == id, includeProperties).FirstOrDefault();
        }

        public IEnumerable<T> Get(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = _dbSet;

            if (filter == null)
            {
                return query.ToList();
            }

            query = query.Where(filter);

            return query.ToList();
        }

        public IEnumerable<T> Get(IEnumerable<string> includeProperties)
        {
            IQueryable<T> query = _dbSet;

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return query.ToList();
        }

        public IEnumerable<T> Get(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy)
        {
            IQueryable<T> query = _dbSet;

            return orderBy(query).ToList();
        }

        public IEnumerable<T> Get(Expression<Func<T, bool>> filter, IEnumerable<string> includeProperties)
        {
            IQueryable<T> query = _dbSet;

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            if (filter == null)
            {
                return query.ToList();
            }

            query = query.Where(filter);

            return query.ToList();
        }

        public IEnumerable<T> Get(
            Expression<Func<T, bool>> filter,
            IEnumerable<string> includeProperties,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy)
        {
            IQueryable<T> query = _dbSet;

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }

            return query.ToList();
        }

        public async Task<T> Add(T entity)
        {
            await _dbSet.AddAsync(entity);
            await Save();
            await _context.Entry(entity).GetDatabaseValuesAsync();

            await Save();

            return entity;
        }

        public async Task<T> Update(T entity)
        {

            entity.UpdatedAt = DateTime.Now;
            
            _context.Entry(entity).State = EntityState.Detached;
            _context.ChangeTracker.Clear();
            _dbSet.Update(entity);
            await Save();
            return entity;
        }
        
        public async Task<T> Update(int id, T entity)
        {

            entity.UpdatedAt = DateTime.Now;

            T existing = await _dbSet.FindAsync(id);

            if (existing != null)
            {
                _context.Entry(existing).CurrentValues.SetValues(entity);
               await  _context.SaveChangesAsync();
            }

            return existing;
        }

        public async Task Delete(int id)
        {
            var entity = await Get(id);

            if (entity == null)
            {
                return;
            }


            entity.DeletedAt = DateTime.Now;

            _dbSet.Remove(entity);
            await Save();


            await Save();
        }

        public async Task Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }


            _dbSet.Remove(entity);
            await Save();


            await Save();
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public void Detach(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                _context.Entry(entity).State = EntityState.Detached;
            }
        }

        public void Detach(T entity)
        {
            _context.Entry(entity).State = EntityState.Detached;
        }
    }
}