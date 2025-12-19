using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using VideoGameApi.Data;
using VideoGameApi.Models;

namespace VideoGameApi.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly VideoGameDbContext _context;
        private readonly DbSet<T> _dbSet;
        public Repository(VideoGameDbContext Context)
        {
            _context = Context;
            _dbSet = _context.Set<T>();
        }
        public async Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _dbSet;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return await query.ToListAsync();
        }
        public async Task<T?> GetByIdAsync(int id, params Expression<Func<T, object>>[] includeProperties)
        {

            IQueryable<T> query = _dbSet;

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }

            return await query.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }
        public void Update(T entity)
        {
            var existingEntity = _dbSet.Find(entity.Id);
            if (existingEntity != null)
            {
                _context.Entry(existingEntity).CurrentValues.SetValues(entity);
            }
        }
        public void SoftDelete(T entity)
        {
            entity.IsDeleted = true;
            Update(entity);
        }
        public async Task<T?> GetByIdIncludingDeletedAsync(int id)
        {
            return await _dbSet.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == id);
        }
        public void Restore(T entity)
        {
            entity.IsDeleted = false;
            Update(entity);
        }
    }
}