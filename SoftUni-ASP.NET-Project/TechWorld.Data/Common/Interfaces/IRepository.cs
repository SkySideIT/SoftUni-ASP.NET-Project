using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TechWorld.Data.Common.Interfaces
{
    public interface IRepository
    {
        Task<IEnumerable<T>> GetAllAsync<T>() where T : class;
        Task<IEnumerable<T>> GetAllAsync<T>(Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[] includes) where T : class;
        Task<T?> GetByIdAsync<T>(object id) where T : class;
        Task<IEnumerable<T>> FindAsync<T>(Expression<Func<T, bool>> predicate) where T : class;
        Task<T?> GetSingleAsync<T>(Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[] includes) where T : class;
        Task AddAsync<T>(T entity) where T : class;
        void Update<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task SaveChangesAsync();

    }
}
