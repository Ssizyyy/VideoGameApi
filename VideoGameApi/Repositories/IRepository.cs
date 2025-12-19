using System.Linq.Expressions;
using VideoGameApi.Models;

namespace VideoGameApi.Repositories
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includeProperties);
        Task<T?> GetByIdAsync(int id, params Expression<Func<T, object>>[] includeProperties);
        Task AddAsync(T entity);
        void Update(T entity);
        void SoftDelete(T entity);
    }
}
