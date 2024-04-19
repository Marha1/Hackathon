using Domain.Enities;
using Infrastructure.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DAL.Repository;

/// <summary>
///     Реализация базового интерфейса
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
{
    private readonly AplicationContext _context;

    public BaseRepository(AplicationContext aplicationContext)
    {
        _context = aplicationContext;
    }

    /// <summary>
    ///     Добавление
    /// </summary>
    /// <param name="entity"></param>
    public async Task Add(T entity)
    {
        await _context.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    ///     Обновление
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public async Task<bool> Update(T entity)
    {
        var findEntity = await _context.Set<T>().FirstOrDefaultAsync(e => e.Id == entity.Id);
        if (findEntity != null)
        {
            _context.Entry(findEntity).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        return false;
    }

    /// <summary>
    ///     Удаление по Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<bool> Delete(Guid id)
    {
        var entity = await _context.Set<T>().FindAsync(id);
        if (entity == null) return false;
        _context.Set<T>().Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    /// <summary>
    ///     Получение всех сущностей T типа
    /// </summary>
    /// <returns>Ответ содержащий все объекты T типа </returns>
    public async Task<IEnumerable<T>> GetAll()
    {
        return await _context.Set<T>().ToListAsync();
    }
}