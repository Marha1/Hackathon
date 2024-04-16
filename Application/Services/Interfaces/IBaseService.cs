using Application.Dtos.UserDto;
using Application.Dtos.UserDto.Request;

namespace Application.Services.Interfaces;
public interface IBaseService<TEntity> 
{
    public Task<UserGetAllResponse> GetAll();
    public Task<UserCreateResponse> Add(UserCreateRequest entity);
    public Task<bool> Update(UserUpdateRequest entity);
    public Task<bool> Delete(Guid id);
}