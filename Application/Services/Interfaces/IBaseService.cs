using Application.Dtos.UserDto.Request;
using Application.Dtos.UserDto.Responce;

namespace Application.Services.Interfaces;

/// <summary>
///     Базовый интерфейс сервиса.
/// </summary>
public interface IBaseService
{
    /// <summary>
    ///     Получает всех пользователей.
    /// </summary>
    /// <returns>Ответ с информацией о всех пользователях.</returns>
    public Task<UserGetAllResponse> GetAll();

    /// <summary>
    ///     Добавляет нового пользователя.
    /// </summary>
    /// <param name="entity">Запрос на создание пользователя.</param>
    /// <returns>Ответ с информацией о созданном пользователе.</returns>
    public Task<UserCreateResponse> Add(UserCreateRequest entity);

    /// <summary>
    ///     Обновляет информацию о пользователе.
    /// </summary>
    /// <param name="entity">Запрос на обновление информации о пользователе.</param>
    /// <returns>Результат обновления информации о пользователе.</returns>
    public Task<bool> Update(UserUpdateRequest entity);

    /// <summary>
    ///     Удаляет пользователя по его идентификатору.
    /// </summary>
    /// <param name="id">Id пользователя.</param>
    /// <returns>Результат удаления пользователя.</returns>
    public Task<bool> Delete(Guid id);
}