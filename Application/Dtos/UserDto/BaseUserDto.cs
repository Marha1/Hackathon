namespace Application.Dtos.UserDto;

public class BaseUserDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public bool isAdmins { get; set; }
}
    
