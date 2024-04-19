namespace Application.Dtos.UserDto.Responce;

/// <summary>
///     Дто ответа на получение всех User
/// </summary>
public class UserGetAllResponse
{
    /// <summary>
    ///     Список всех пользователей
    /// </summary>
    public IEnumerable<BaseUserDto> Users { get; set; }
}