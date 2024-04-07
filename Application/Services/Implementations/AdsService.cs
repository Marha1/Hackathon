using Application.Dtos.AdsDto;
using Application.Dtos.UserDto;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Enities;
using Infrastrucure.DAL.Interfaces;

namespace Application.Services.Implementations;

public class AdsService: IAdsService
{
    private readonly IAdsRepository<Ads> _adsRepository;
    private readonly IUserRepository<User> _user;
    private readonly IMapper _mapper;

    public AdsService(IAdsRepository<Ads> adsRepository, IMapper mapper,IUserRepository<User> user)
    {
        _adsRepository = adsRepository;
        _mapper = mapper;
        _user = user;
    }

    public AdsGetAllResponce GetAll()
    {
        var ads=_adsRepository.GetAll();
        return _mapper.Map<AdsGetAllResponce>(ads);
    }

    public AdsCreateResponse Add(Ads entity)
    {
        var user = _user.FindById(entity.UserId);
        if (user is null)
        {
            return null;
        }
        user.Ads.Add(entity);
        return _mapper.Map<AdsCreateResponse>(user);
    }

    public bool Update(Ads entity)
    {
        return _adsRepository.Update(entity);
    }

    public bool Delete(Guid id)
    {
        return _adsRepository.Delete(id); 
    }

   

    public bool TryToPublic(Guid id)
    {
        return _adsRepository.CanUserPublish(id);
    }
}