using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TechWorld.Data.Common.Interfaces;

namespace TechWorld.Data.Common
{
    public class Repository : IRepository
    {
        private readonly ApplicationDbContext _context;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync<T>(T entity) where T : class
            => await _context.Set<T>().AddAsync(entity);

        public void Delete<T>(T entity) where T : class
            => _context.Set<T>().Remove(entity);

        public async Task<IEnumerable<T>> FindAsync<T>(Expression<Func<T, bool>> predicate) where T : class
            => await _context.Set<T>().Where(predicate).ToListAsync();

        public async Task<T?> GetSingleAsync<T>(Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[] includes) where T : class
        {
            IQueryable<T> query = _context.Set<T>();

            if (includes != null)
            {
                foreach (var include in includes)
                    query = query.Include(include);
            }

            return await query.FirstOrDefaultAsync(predicate);
        }
        public async Task<IEnumerable<T>> GetAllAsync<T>(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes) where T : class
        {
            IQueryable<T> query = _context.Set<T>();

            if (includes != null)
            {
                foreach (var include in includes)
                    query = query.Include(include);
            }

            return await query.Where(predicate).ToListAsync();
        }
        public async Task<IEnumerable<T>> GetAllAsync<T>() where T : class
            => await _context.Set<T>().ToListAsync();

        public async Task<T?> GetByIdAsync<T>(object id) where T : class
            => await _context.Set<T>().FindAsync(id);

        public async Task SaveChangesAsync()
            => await _context.SaveChangesAsync();

        public void Update<T>(T entity) where T : class
            => _context.Set<T>().Update(entity);
    }
}
