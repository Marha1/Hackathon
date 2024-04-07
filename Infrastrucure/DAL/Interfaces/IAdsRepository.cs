using Infrastructure.Dal.Interfaces;
namespace Infrastrucure.DAL.Interfaces;

public interface IAdsRepository<Ads> : IBaseRepository<Ads>
{
    public bool CanUserPublish(Guid userId);
}
   