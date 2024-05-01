using Application.Dtos.UserDto.Responce;

namespace Application.Services.Interfaces;

/// <summary>
///     Интерфейс сервиса пользователей.
/// </summary>
public interface IUserService : IBaseService
{
    /// <summary>
    ///     Находит пользователя по его Id.
    /// </summary>
    /// <param name="id">Id пользователя.</param>
    /// <returns>Ответ с информацией о найденном пользователе.</returns>
    public Task<UserGetByIdResponse> FindById(Guid id);
}