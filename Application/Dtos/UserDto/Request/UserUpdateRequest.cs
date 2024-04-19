namespace Application.Dtos.UserDto.Request;

/// <summary>
///     Дто запроса обновления пользовател
/// </summary>
public class UserUpdateRequest
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
    public bool isAdmins { get; set; }
}