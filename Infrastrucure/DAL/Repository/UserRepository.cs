using Domain.Enities;
using Infrastrucure.DAL.Interfaces;

namespace Infrastrucure.DAL.Repository;
/// <summary>
/// Реализация расширяющего интерфейса IEmployeRepository
/// </summary>
public class UserRepository : BaseRepository<User>,IUserRepository<User>
{
    private readonly AplicationContext _context;
    public UserRepository(AplicationContext context):base (context)
    {
        this._context = context;
    }

    public User FindById(Guid id)
    {
        var person = _context.Persons.FirstOrDefault(x => x.Id == id);
        return person;
    }
}