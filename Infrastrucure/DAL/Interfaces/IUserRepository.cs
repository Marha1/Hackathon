
using Infrastructure.Dal.Interfaces;
namespace Infrastrucure.DAL.Interfaces;
public interface IUserRepository<User>:IBaseRepository<User>
{
    public User FindById(Guid id);
}