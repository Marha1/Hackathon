using Domain.Enities;

namespace Infrastructure.DAL.Interfaces;

/// <summary>
///     Базовый интерфейс CRUD.
/// </summary>
/// <typeparam name="T">Тип сущности, производный от BaseEntity.</typeparam>
 public interface IBaseRepository<T> where T : BaseEntity
{
    /// <summary>
    ///     Добавляет сущность в хранилище.
    /// </summary>
    /// <param name="entity">Сущность для добавления.</param>
    Task Add(T entity);

    /// <summary>
    ///     Обновляет сущность в хранилище.
    /// </summary>
    /// <param name="entity">Сущность для обновления.</param>
    Task<bool> Update(T entity);

    /// <summary>
    ///     Удаляет сущность из хранилища по её идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор сущности.</param>
    Task<bool> Delete(Guid id);

    /// <summary>
    ///     Получает все сущности из хранилища.
    /// </summary>
    /// <returns>Коллекция всех сущностей.</returns>
    Task<IEnumerable<T>> GetAll();
}