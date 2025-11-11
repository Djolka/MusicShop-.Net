using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicShop.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(object id);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<int> DeleteAllAsync(IEnumerable<T>? entities = null);
        Task SaveChangesAsync();
    }
}
