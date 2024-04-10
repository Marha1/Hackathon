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

    public IEnumerable<AdsGetAllResponce> GetAll()
    {
        var ads = _adsRepository.GetAll().Select(ad =>
        {
            var adDto = new AdsGetAllResponce()
            {
                Id = ad.Id,
               Text = ad.Text,
                UserId = ad.UserId,
            };
            return adDto;
        });

        return ads;
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
        if (entity is null)
        {
            return false;
        }
        entity.UserId = Guid.Empty;
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

    public Ads GetById(Guid id)
    {
        return _adsRepository.GetById(id);
    }

    public IEnumerable<AdsGetByTextResponce> FindByText(string Text)
    {
        var ads=_adsRepository.GetAll().Where(ad => ad.Text == Text);
        if (ads is null)
        {
            return null;
        }
        var adsDtoList = ads.Select(ad => new AdsGetByTextResponce
        {
            Id = ad.Id,
            Text = ad.Text,
            Number = ad.Number,
            Created = ad.Created,
            ExpirationDate = ad.ExpirationDate,
            Rating = ad.Rating
        }).ToList();
        return adsDtoList;
    }
}