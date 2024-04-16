using Application.Dtos.AdsDto;
using Application.Dtos.AdsDto.Request;
using Application.Dtos.AdsDto.Responce;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Enities;
using Infrastructure.DAL.Interfaces;

namespace Application.Services.Implementations
{
    public class AdsService : IAdsService
    {
        private readonly IAdsRepository<Ads> _adsRepository;
        private readonly IUserRepository<User> _userRepository;
        private readonly IMapper _mapper;

        public AdsService(IAdsRepository<Ads> adsRepository, IMapper mapper, IUserRepository<User> userRepository)
        {
            _adsRepository = adsRepository;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<AdsGetAllResponce>> GetAll()
        {
            var ads = await _adsRepository.GetAll();
            var adDtos = ads.Select(ad => _mapper.Map<AdsGetAllResponce>(ad));
            return adDtos;
        }

        public async Task<AdsCreateResponse> Add(AdsCreateRequest entity)
        {
            var adsToAdd = _mapper.Map<Ads>(entity);
            var user = await _userRepository.FindById(entity.UserId);
            if (user is null)
            {
                return null;
            }
            await _adsRepository.Add(adsToAdd);

             user.Ads.Add(adsToAdd);

            return _mapper.Map<AdsCreateResponse>(adsToAdd);
        }

        public async Task<bool> Update(AdsUpdateRequest entity)
        {
            var adsToAdd = _mapper.Map<Ads>(entity);
            if (entity is null)
            {
                return false;
            }
            adsToAdd.UserId = Guid.Empty;
            return await _adsRepository.Update(adsToAdd);
        }

        public async Task<bool> Delete(Guid id)
        {
            return await _adsRepository.Delete(id);
        }

        public bool TryToPublic(Guid id)
        {
            return _adsRepository.CanUserPublish(id);
        }

        public async Task<Ads> GetById(Guid id)
        {
            return await _adsRepository.GetById(id);
        }

        public async Task<IEnumerable<AdsGetByTextResponce>> FindByText(string text)
        {
            var ads = await _adsRepository.GetAll();
            if (ads is null)
            {
                return null;
            }
            var adsWhere = ads.Where(ad => ad.Text == text).ToList();
            var adsDtoList = adsWhere.Select(ad => _mapper.Map<AdsGetByTextResponce>(ad)).ToList();
            return adsDtoList;
        }

        public async Task<IEnumerable<AdsDescendingFiltrationResponce>> Filtration()
        {
            var ads = await _adsRepository.GetAll();
            if (ads is null)
            {
                return null;
            }
            var adsOrder = ads.OrderByDescending(n => n.Rating).Where(n => n.Rating != 0);
            var adsDtoList = adsOrder.Select(ad => _mapper.Map<AdsDescendingFiltrationResponce>(ad)).ToList();
            return adsDtoList;
        }

        public async Task<IEnumerable<AdsAscendingFiltrationResponce>> AscendingFiltration()
        {
            var ads = await _adsRepository.GetAll();
            if (ads is null)
            {
                return null;
            }
            var adsOrder = ads.OrderBy(n => n.Rating).Where(n => n.Rating != 0);
            var adsDtoList = adsOrder.Select(ad => _mapper.Map<AdsAscendingFiltrationResponce>(ad)).ToList();
            return adsDtoList;
        }
    }
}
