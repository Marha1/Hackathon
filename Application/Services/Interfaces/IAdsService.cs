using Application.Dtos.AdsDto;
using Domain.Enities;

namespace Application.Services.Interfaces;

public interface IAdsService
{
      
    public AdsGetAllResponce GetAll();
    public AdsCreateResponse Add(Ads entity);
    public bool Update(Ads entity);
    public bool Delete(Guid id);
    public bool TryToPublic(Guid id);
}