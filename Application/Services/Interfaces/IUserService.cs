using Application.Dtos.UserDto;
using Domain.Enities;

namespace Application.Services.Interfaces;

public interface IUserService : IBaseService<User>
{
    public UserGetByIdResponse FindById(Guid id);
}
   