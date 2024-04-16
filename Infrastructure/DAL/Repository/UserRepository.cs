using Domain.Enities;
using Infrastructure.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DAL.Repository;
/// <summary>
/// Реализация расширяющего интерфейса IUserRepository
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
        
        var users = _context.Users.Include(x=>x.Ads).FirstOrDefault(x => x.Id == id);
        if (users is not null)
        {
            return users;
        }

        if (_context.Users.FirstOrDefault(x => x.Id == id) is not null)
        {
            return _context.Users.FirstOrDefault(x => x.Id == id);
        }
        return null;
    }
}