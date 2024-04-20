namespace Application.Dtos.UserDto.Request;

/// <summary>
///     Дто запроса на создание пользователя
/// </summary>
public class UserCreateRequest
{
    /// <summary>
    ///     Id пользователя
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    ///     Имя пользователя
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    ///     Статус администратора у пользователя
    /// </summary>
    public bool Admin { get; set; }
}