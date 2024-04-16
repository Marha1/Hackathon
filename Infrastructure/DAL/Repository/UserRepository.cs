using Infrastructure.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Domain.Enities;

namespace Infrastructure.DAL.Repository
{
    /// <summary>
    /// Реализация расширяющего интерфейса IUserRepository
    /// </summary>
    public class UserRepository : BaseRepository<User>, IUserRepository<User>
    {
        private readonly AplicationContext _context;

        public UserRepository(AplicationContext context) : base(context)
        {
            _context = context;
        }

        public async Task<User> FindById(Guid id)
        {
            var user = await _context.Users.Include(x => x.Ads).FirstOrDefaultAsync(x => x.Id == id);
            return user;
        }
    }
}