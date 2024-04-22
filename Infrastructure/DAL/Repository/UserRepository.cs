using Domain.Enities;
using Infrastructure.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DAL.Repository;

/// <summary>
///     Реализация расширяющего интерфейса IUserRepository
/// </summary>
public class UserRepository : BaseRepository<User>, IUserRepository
{
    private readonly AplicationContext _context;

    public UserRepository(AplicationContext context) : base(context)
    {
        _context = context;
    }

    /// <summary>
    ///     Поиск пользователя по Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Ответ содержащий информацию о пользователе</returns>
    public async Task<User> FindById(Guid id)
    {
        var user = await _context.Users.Include(x => x.Ads).FirstOrDefaultAsync(x => x.Id == id);
        return user;
    }
}