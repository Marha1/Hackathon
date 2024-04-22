using Domain.Enities;
using Infrastructure.DAL.Configurations;
using Infrastructure.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Infrastructure.DAL.Repository;

/// <summary>
///     Реализация расширяющего интерфейса объявления
/// </summary>
public class AdsRepository : BaseRepository<Ads>, IAdsRepository
{
    private readonly AplicationContext _context;
    private readonly IOptions<AppSettings> _options;

    public AdsRepository(AplicationContext context, IOptions<AppSettings> options) : base(context)
    {
        _context = context;
        _options = options;
    }

    /// <summary>
    ///     Попытка добавить обхявление
    /// </summary>
    /// <param name="userId">id пользователя</param>
    /// <returns>True-если возможно добавить,в противном случае-False</returns>
    public async Task<bool> CanUserPublish(Guid userId)
    {
        var user = await _context.Users.Include(u => u.Ads).FirstOrDefaultAsync(x => x.Id == userId);
        if (user == null) return false;

        return user.Ads.Count < _options.Value.MaxAdsPerUser;
    }

    /// <summary>
    ///     Получение объявления по id
    /// </summary>
    /// <param name="id">Id объявления</param>
    /// <returns>Ответ содержащий найденое объявление</returns>
    public async Task<Ads> GetById(Guid id)
    {
        return await _context.Ads.FirstOrDefaultAsync(x => x.Id == id);
    }
}