using Application.Dtos.UserDto;
using Domain.Enities;
namespace Application.Services.Interfaces;
public interface IBaseService<TEntity> 
{
    
    public UserGetAllResponse GetAll();
    public UserCreateResponse Add(TEntity entity);
    public bool Update(TEntity entity);
    public bool Delete(Guid id);
}