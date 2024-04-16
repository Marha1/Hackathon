using Application.Dtos.AppSettings;
using Domain.Enities;
using Infrastructure.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Infrastructure.DAL.Repository;
public class AdsRepository:BaseRepository<Ads>,IAdsRepository<Ads>
{
    private readonly AplicationContext _context;
    private readonly IOptions<AppSettings> _options;
    
    public AdsRepository(AplicationContext context, IOptions<AppSettings> options) : base(context)
    {
         _context = context;
         _options = options;
    }
    
    public bool CanUserPublish(Guid userId)
    {
        var user = _context.Users.Include(u => u.Ads).FirstOrDefault(x => x.Id == userId);
        if (user==null)
        {
            return false;
        }

        return user.Ads.Count < _options.Value.MaxAdsPerUser;
    }
    
    public async Task<Ads> GetById(Guid id)
        {
            return await _context.Ads.FirstOrDefaultAsync(x => x.Id == id);
        }
}