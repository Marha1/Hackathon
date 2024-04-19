namespace Application.Dtos.UserDto.Responce;

/// <summary>
///     Базовое Дто для пользователя
/// </summary>
public class BaseUserDto
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