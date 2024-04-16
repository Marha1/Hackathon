namespace Application.Dtos.UserDto.Request;

public class UserCreateRequest
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public bool isAdmins { get; set; }
}