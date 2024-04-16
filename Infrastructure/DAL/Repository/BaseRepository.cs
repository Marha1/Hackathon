using Infrastructure.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Domain.Enities;

namespace Infrastructure.DAL.Repository
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        private readonly AplicationContext _context;

        public BaseRepository(AplicationContext aplicationContext)
        {
            _context = aplicationContext;
        }

        public async Task Add(T entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> Update(T entity)
        {
            var findEntity = await _context.Set<T>().FirstOrDefaultAsync(e => e.Id == entity.Id);
            if (findEntity != null)
            {
                var objectTypes = _context.Model.FindEntityType(typeof(T));
                var props = objectTypes?.GetProperties();
                if (props != null)
                {
                    foreach (var property in props)
                    {
                        var entityValue = property.PropertyInfo?.GetValue(entity);
                        if (entityValue != null && entityValue.GetType() != typeof(Guid))
                        {
                            property.PropertyInfo?.SetValue(findEntity, entityValue);
                            _context.Entry(findEntity).CurrentValues.SetValues(entityValue);
                        }
                    }
                }
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> Delete(Guid id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity == null)
            {
                return false;
            }
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }
    }
}
