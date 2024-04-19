using Domain.Enities;

namespace Infrastructure.DAL.Interfaces;

/// <summary>
///     Расширяющий интерфейс для объявления
/// </summary>
/// <typeparam name="Ads"></typeparam>
public interface IAdsRepository<Ads> : IBaseRepository<Ads> where Ads : BaseEntity
{
    /// <summary>
    ///     Попытка добавить объявление
    /// </summary>
    /// <param name="userId">Id пользоветеля</param>
    /// <returns>True-если возможно добавить,в противном случае-False</returns>
    public bool CanUserPublish(Guid userId);

    /// <summary>
    ///     Получение объявления по id
    /// </summary>
    /// <param name="id">Id объявления</param>
    /// <returns>Ответ содержащий найденое объявление</returns>
    public Task<Ads> GetById(Guid id);
}