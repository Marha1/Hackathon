    using Application.Dtos.AdsDto.Request;
    using Application.Dtos.AdsDto.Responce;
    using Application.Services.Interfaces;
    using AutoMapper;
    using Domain.Enities;
    using Infrastructure.DAL.Interfaces;

    namespace Application.Services.Implementations;

    /// <summary>
    ///     Реализация сервиса для работы с объявлением.
    /// </summary>
    public class AdsService : IAdsService
    {
        private readonly IAdsRepository _adsRepository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public AdsService(IAdsRepository adsRepository, IMapper mapper, IUserRepository userRepository)
        {
            _adsRepository = adsRepository ?? throw new ArgumentNullException(nameof(adsRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<IEnumerable<AdsGetAllResponce>> GetAll()
        {
            var ads = await _adsRepository.GetAll();
            return ads.Select(ad => _mapper.Map<AdsGetAllResponce>(ad));
        }

        public async Task<AdsCreateResponse> Add(AdsCreateRequest entity)
        {
            var adsToAdd = _mapper.Map<Ads>(entity);
            var user = await _userRepository.FindById(entity.UserId);
            if (user == null)
                throw new ArgumentException("Пользователь не найден", nameof(entity.UserId));
            await _adsRepository.Add(adsToAdd);
            user.Ads.Add(adsToAdd);
            return _mapper.Map<AdsCreateResponse>(adsToAdd);
        }

        public async Task<bool> Update(AdsUpdateRequest entity)
        {
            var ads = await _adsRepository.GetById(entity.Id);
            if (ads == null)
                return false;
            _mapper.Map(entity, ads);
            return await _adsRepository.Update(ads);
        }

        public async Task<bool> Delete(Guid id)
        {
            return await _adsRepository.Delete(id);
        }

        public async Task<bool> TryToPublic(Guid id)
        {
            return await _adsRepository.CanUserPublish(id);
        }
        
        public async Task<IEnumerable<AdsFiltrationResponce>> Filtration(bool ascending,string text)
        {
            var ads = await _adsRepository.GetAll();
            if (ads == null)
            {
                return null;
            }
            if (!string.IsNullOrEmpty(text))
            {
                ads = ads.Where(ad => ad.Text.Contains(text));
            }

            var orderedAds = ascending ? ads.OrderBy(n => n.Rating) : ads.OrderByDescending(n => n.Rating);

            var adsDtoList = orderedAds.Select(ad => _mapper.Map<AdsFiltrationResponce>(ad)).ToList();
        
            return adsDtoList;
        }
    }