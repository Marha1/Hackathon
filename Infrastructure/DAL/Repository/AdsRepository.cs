using Domain.Enities;
using Domain.Primitives;
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
        var user = await _context.Users.AsQueryable().Include(u => u.Ads).FirstOrDefaultAsync(x => x.Id == userId);
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
        var ads = await _context.Ads.AsQueryable().FirstOrDefaultAsync(x => x.Id == id);
        if (ads is null) return null;
        return ads;
    }

    /// <summary>
    ///     Фильтрация объявлений
    /// </summary>
    /// <param name="ascending">по возрастанию или убыванию</param>
    /// <param name="sortBy">Вид сортировки</param>
    /// <param name="text">Для поиска объявления по тексту</param>
    /// <returns>Найденое объявление с учетом треб.сортировки</returns>
    public async Task<IEnumerable<Ads>> Filtration(bool ascending, SortBy sortBy, string text)
    {
        var ads = _context.Ads.AsQueryable();

        if (!string.IsNullOrEmpty(text)) ads = ads.Where(ad => ad.Text.Contains(text));

        switch (sortBy)
        {
            case SortBy.Rating:
                ads = ascending ? ads.OrderBy(n => n.Rating) : ads.OrderByDescending(n => n.Rating);
                break;
            case SortBy.DateOFCreate:
                ads = ascending ? ads.OrderBy(n => n.Created) : ads.OrderByDescending(n => n.Created);
                break;
        }

        return await ads.ToListAsync();
    }
}