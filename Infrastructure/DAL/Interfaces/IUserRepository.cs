
using Domain.Enities;

namespace Infrastructure.DAL.Interfaces;
public interface IUserRepository<User>:IBaseRepository<User> where User : BaseEntity
{
    public Task<User> FindById(Guid id);
}