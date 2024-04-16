using Application.Dtos.UserDto;
namespace Application.Services.Interfaces;
public interface IBaseService<TEntity> 
{
    
    public Task<UserGetAllResponse> GetAll();
    public UserCreateResponse Add(TEntity entity);
    public bool Update(TEntity entity);
    public bool Delete(Guid id);
}