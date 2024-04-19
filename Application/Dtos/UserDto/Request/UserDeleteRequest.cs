namespace Application.Dtos.UserDto.Request;

/// <summary>
///     Дто запроса удаление пользователя
/// </summary>
public class UserDeleteRequest
{
    /// <summary>
    ///     Id пользователя
    /// </summary>
    public Guid Id { get; set; }
}