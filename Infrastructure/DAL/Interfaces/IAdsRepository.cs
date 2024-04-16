namespace Infrastructure.DAL.Interfaces;

public interface IAdsRepository<Ads> : IBaseRepository<Ads>
{
    public bool CanUserPublish(Guid userId);
    public Ads GetById(Guid id);
}
   