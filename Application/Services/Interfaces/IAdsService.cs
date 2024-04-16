using Application.Dtos.AdsDto.Request;
using Application.Dtos.AdsDto.Responce;
using Domain.Enities;

namespace Application.Services.Interfaces;
public interface IAdsService
{
      
    public Task<IEnumerable<AdsGetAllResponce>> GetAll();
    public Task<AdsCreateResponse> Add(AdsCreateRequest entity);
    public Task<bool> Update(AdsUpdateRequest entity);
    public Task<bool> Delete(Guid id);
    public bool TryToPublic(Guid id);
    public Task<Ads> GetById(Guid id);
    public Task<IEnumerable<AdsGetByTextResponce>> FindByText(string Name);
    public Task<IEnumerable<AdsDescendingFiltrationResponce>> Filtration();
    public Task<IEnumerable<AdsAscendingFiltrationResponce>> AscendingFiltration();
}