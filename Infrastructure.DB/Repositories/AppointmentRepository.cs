using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Domain.Models;
using Core.DomainServices.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Core.Infrastructure.Repositories
{
    public class AppointmentRepository : DatabaseRepository<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(DbContext context) : base(context)
        {
        }

        public new IEnumerable<Appointment> Get()
        {
            return _dbSet.FromSqlRaw("SELECT * FROM [FysioData].[dbo].[Appointments] where isTreatment = 0").ToList();
        }

        public new  async Task<IEnumerable<Appointment>> GetAsync()
        {
            return await _dbSet.FromSqlRaw("SELECT * FROM [FysioData].[dbo].[Appointments] where isTreatment = 0").AsNoTracking().ToListAsync();
        }
        
        public new async Task<Appointment> Get(int id)
        {
            return await _dbSet.FromSqlRaw("SELECT * FROM [FysioData].[dbo].[Appointments] where isTreatment = 0").SingleOrDefaultAsync(e => e.Id == id);
        }
        
        public new Appointment Get(int id, IEnumerable<string> includeProperties)
        {
            return Get(e => e.Id == id, includeProperties).FirstOrDefault();
        }
        
        public new IEnumerable<Appointment> Get(Expression<Func<Appointment, bool>> filter)
        {
            IQueryable<Appointment> query = _dbSet.FromSqlRaw("SELECT * FROM [FysioData].[dbo].[Appointments] where isTreatment = 0");
        
            if (filter == null)
            {
                return query.ToList();
            }
        
            query = query.Where(filter);
        
            return query.ToList();
        }
        
        public new IEnumerable<Appointment> Get(IEnumerable<string> includeProperties)
        {
            IQueryable<Appointment> query = _dbSet.FromSqlRaw("SELECT * FROM [FysioData].[dbo].[Appointments] where isTreatment = 0");
        
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
        
            return query.ToList();
        }
        
        public new IEnumerable<Appointment> Get(Func<IQueryable<Appointment>, IOrderedQueryable<Appointment>> orderBy)
        {
            IQueryable<Appointment> query = _dbSet.FromSqlRaw("SELECT * FROM [FysioData].[dbo].[Appointments] where isTreatment = 0");
        
            return orderBy(query).ToList();
        }
        
        public new IEnumerable<Appointment> Get(Expression<Func<Appointment, bool>> filter, IEnumerable<string> includeProperties)
        {
            IQueryable<Appointment> query = _dbSet.FromSqlRaw("SELECT * FROM [FysioData].[dbo].[Appointments] where isTreatment = 0");
        
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
        
        public new IEnumerable<Appointment> Get(
            Expression<Func<Appointment, bool>> filter,
            IEnumerable<string> includeProperties,
            Func<IQueryable<Appointment>, IOrderedQueryable<Appointment>> orderBy)
        {
            IQueryable<Appointment> query = _dbSet.FromSqlRaw("SELECT * FROM [FysioData].[dbo].[Appointments] where isTreatment = 0");
        
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
    }
}