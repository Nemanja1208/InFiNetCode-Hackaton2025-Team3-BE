
using System.Linq.Expressions; // Add this
using System.Collections.Generic; // Add this
using System.Threading.Tasks; // Add this
using System; // Add this

namespace Application_Layer.Common.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(Guid id);
        Task<T?> GetByIdAsync(Guid id, params Expression<Func<T, object>>[] includes); // Keep existing overload for direct includes
        Task<T?> GetByIdAsync(Guid id, Func<IQueryable<T>, IQueryable<T>> includeQuery); // New overload for complex includes
        Task<IEnumerable<T>> GetAllAsync();
        Task CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
