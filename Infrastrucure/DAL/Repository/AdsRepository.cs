using Domain.Enities;
using Infrastrucure.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastrucure.DAL.Repository;

public class AdsRepository:BaseRepository<Ads>,IAdsRepository<Ads>
{/// <summary>
 /// Todo:Передать параметр из appsettings
 /// </summary>
    private readonly AplicationContext _context;
    private readonly int _maxAdsPerUser;
    public AdsRepository(AplicationContext context,IConfiguration configuration):base(context)
    {
        this._context = context;
    }

    public bool CanUserPublish(Guid userId)
    {
        var user = _context.Users.Include(u => u.Ads).FirstOrDefault(x => x.Id == userId);
        if (user==null)
        {
            return false;
        }

        return user.Ads.Count < _maxAdsPerUser;
    }
}