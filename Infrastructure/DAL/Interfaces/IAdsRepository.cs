using Domain.Enities;
using Domain.Primitives;

namespace Infrastructure.DAL.Interfaces;

/// <summary>
///     Расширяющий интерфейс для объявления
/// </summary>
public interface IAdsRepository : IBaseRepository<Ads>
{
    /// <summary>
    ///     Попытка добавить объявление
    /// </summary>
    /// <param name="userId">Id пользоветеля</param>
    /// <returns>True-если возможно добавить,в противном случае-False</returns>
    public Task<bool> CanUserPublish(Guid userId);

    /// <summary>
    ///     Получение объявления по id
    /// </summary>
    /// <param name="id">Id объявления</param>
    /// <returns>Ответ содержащий найденое объявление</returns>
    public Task<Ads> GetById(Guid id);

    /// <summary>
    ///     Фильтрация объявлений
    /// </summary>
    /// <param name="ascending">по возрастанию или убыванию</param>
    /// <param name="sortBy">Вид сортировки</param>
    /// <param name="text">Для поиска объявления по тексту</param>
    /// <returns></returns>
    public Task<IEnumerable<Ads>> Filtration(bool ascending, SortBy sortBy, string text);
}