using System;
using Application.Dtos;
using Application.Dtos.AdsDto;
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
        var ads = _adsRepository.GetAll();
        var users = _user.GetAll(); 

        var adsDto = new AdsGetAllResponce
        {
            Ads = ads,
            User = _mapper.Map<IEnumerable<User>, IEnumerable<BaseUserDto>>(users) 
        };

        return adsDto;
    }

    public AdsCreateResponse Add(Ads entity)
    {
        
        var user = _user.FindById(entity.UserId);
        if (user is null)
        {
            return null;
        }
        _adsRepository.Add(entity);
        user.Ads.Add(entity);
        return _mapper.Map<AdsCreateResponse>(entity);
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