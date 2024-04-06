using Application.Dtos.UserDto;
using Domain.Enities;
namespace Application.Services.Interfaces;
public interface IBaseService<TEntity>
{
    public UserGetByIdResponse FindById(Guid id);
    public UserGetAllResponse GetAll();
    public UserCreateResponse Add(TEntity entity);
    public UserUpdateResponse Update(TEntity entity);
    public bool Delete(Guid id);
}