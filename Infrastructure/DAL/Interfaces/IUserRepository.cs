
namespace Infrastructure.DAL.Interfaces;
public interface IUserRepository<User>:IBaseRepository<User>
{
    public User FindById(Guid id);
}