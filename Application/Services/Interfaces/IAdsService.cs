using Application.Dtos.AdsDto.Request;
using Application.Dtos.AdsDto.Responce;
using Domain.Primitives;

namespace Application.Services.Interfaces;

/// <summary>
///     Сервис для работы с объявлениями.
/// </summary>
public interface IAdsService
{
    /// <summary>
    ///     Получает все объявления.
    /// </summary>
    /// <returns>Список всех объявлений.</returns>
    public Task<IEnumerable<AdsGetAllResponce>> GetAll();

    /// <summary>
    ///     Добавляет новое объявление.
    /// </summary>
    /// <param name="entity">Запрос на создание объявления.</param>
    /// <returns>Ответ с информацией о созданном объявлении.</returns>
    public Task<AdsCreateResponse> Add(AdsCreateRequest entity);

    /// <summary>
    ///     Обновляет информацию об объявлении.
    /// </summary>
    /// <param name="entity">Запрос на обновление информации об объявлении.</param>
    /// <returns>Результат обновления информации об объявлении.</returns>
    public Task<bool> Update(AdsUpdateRequest entity);

    /// <summary>
    ///     Удаляет объявление по его идентификатору.
    /// </summary>
    /// <param name="id">Id объявления.</param>
    /// <returns>Результат удаления объявления.</returns>
    public Task<bool> Delete(Guid id);
    
    /// <summary>
    ///     Фильтрация объявлений
    /// </summary>
    /// <param name="ascending">по возрастанию или убыванию</param>
    /// <param name="sortBy">Вид сортировки</param>
    /// <param name="text">Для поиска объявления по тексту</param>
    /// <returns></returns>
    public Task<IEnumerable<AdsFiltrationResponce>> Filtration(bool choose, SortBy sort, string text);

    /// <summary>
    ///     Поиск по Id
    /// </summary>
    /// <param name="id">Id объявления</param>
    /// <returns>Ответ с информацией о найденном объявлении</returns>
    public Task<AdsGetAllResponce> FindById(Guid id);
}