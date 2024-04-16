using Domain.Enities;

namespace Infrastructure.DAL.Interfaces;

public interface IAdsRepository<Ads> : IBaseRepository<Ads> where Ads : BaseEntity
{
    public bool CanUserPublish(Guid userId);
    public Task<Ads> GetById(Guid id);
}
   