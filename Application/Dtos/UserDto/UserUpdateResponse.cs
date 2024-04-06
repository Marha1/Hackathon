namespace Application.Dtos.UserDto;

/// <summary>
/// Дто ответа на обновление Person
/// </summary>
public class UserUpdateResponse : BaseUserDto
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; init; }
}
