using Application.Dtos.AdsDto.Request;
using Application.Dtos.AdsDto.Responce;

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
    ///     Попытка опубликовать объявление с проверкой кол-ва объявлений по Id пользователя.
    /// </summary>
    /// <param name="id">Идентификатор объявления.</param>
    /// <returns>Результат попытки публикации объявления.</returns>
    public  Task<bool> TryToPublic(Guid id);

    /// <summary>
    ///     Ищет объявления по тексту.
    /// </summary>
    /// <param name="Name">Текст для поиска.</param>
    /// <returns>Список объявлений, соответствующих критериям поиска.</returns>
    public Task<IEnumerable<AdsGetByTextResponce>> FindByText(string Name);

    /// <summary>
    ///     Фильтрует объявления .
    /// </summary>
    /// <returns>Список отфильтрованных объявлений.</returns>
    public Task<IEnumerable<AdsFiltrationResponce>> Filtration(bool choose);
}