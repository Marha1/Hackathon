using Domain.Enities;
using Infrastrucure.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using Microsoft.Extensions.Configuration.Json;

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
        _maxAdsPerUser = Convert.ToInt32(configuration["AppSettings:MaxAdsPerUser"]);

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

    public Ads GetById(Guid id)
    {
        var ads = _context.Ads.FirstOrDefault(x => x.Id == id);
        if (ads is null)
        {
            return null;
        }

        return ads;
        
    }
}