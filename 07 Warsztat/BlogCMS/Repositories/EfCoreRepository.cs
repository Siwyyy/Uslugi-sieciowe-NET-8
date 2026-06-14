using System.Collections.Generic;
using System.Threading.Tasks;
using BlogCMS.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogCMS.Repositories
{
    public class EfCoreRepository<T> : IRepository<T> where T : class
    {
        private readonly BlogDbContext _context;
        private DbSet<T> _entities;

        public EfCoreRepository(BlogDbContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync() => await _entities.ToListAsync();

        public async Task<T> GetByIdAsync(int id) => await _entities.FindAsync(id);

        public async Task<int> AddAsync(T entity)
        {
            await _entities.AddAsync(entity);
            await _context.SaveChangesAsync();
            
            var propertyInfo = entity.GetType().GetProperty("Id");
            if (propertyInfo != null)
            {
                return (int)propertyInfo.GetValue(entity);
            }
            return 0;
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            _entities.Update(entity);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null)
                return false;

            _entities.Remove(entity);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}
