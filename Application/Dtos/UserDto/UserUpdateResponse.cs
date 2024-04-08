using System;

namespace Application.Dtos.UserDto;

/// <summary>
/// Дто ответа на обновление User
/// </summary>
public class UserUpdateResponse : BaseUserDto
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; init; }
}
