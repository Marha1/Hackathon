using System;
using Application.Dtos.AdsDto;
using Domain.Enities;

namespace Application.Services.Interfaces;

public interface IAdsService
{
      
    public IEnumerable<AdsGetAllResponce> GetAll();
    public AdsCreateResponse Add(Ads entity);
    public bool Update(Ads entity);
    public bool Delete(Guid id);
    public bool TryToPublic(Guid id);
    public Ads GetById(Guid id);
    public IEnumerable<AdsGetByTextResponce> FindByText(string Name);
    public IEnumerable<AdsDescendingFiltrationResponce> Filtration();
    public IEnumerable<AdsAscendingFiltrationResponce> AscendingFiltration();
}