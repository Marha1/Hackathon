using System.Collections.Generic;

namespace Application.Dtos.UserDto;

/// <summary>
/// Дто ответа на получение всех User
/// </summary>
public class UserGetAllResponse 
{
    public IEnumerable<BaseUserDto> Users { get; set; }
}
    