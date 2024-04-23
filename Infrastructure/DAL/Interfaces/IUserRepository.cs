using Domain.Enities;

namespace Infrastructure.DAL.Interfaces;

/// <summary>
///     Расширяющий интерфейс для пользователя
/// </summary>
public interface IUserRepository : IBaseRepository<User>
{
    /// <summary>
    ///     Поиск пользователя по Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Ответ содержащий информацию о найденом пользователе</returns>
    public Task<User> FindById(Guid id);
}