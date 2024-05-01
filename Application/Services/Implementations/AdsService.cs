using Application.Dtos.AdsDto.Request;
using Application.Dtos.AdsDto.Responce;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Enities;
using Domain.Primitives;
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

    /// <summary>
    ///     Получение всех объявлений.
    /// </summary>
    /// <returns>Ответ с информацией обо всех объявлениях.</returns>
    public async Task<IEnumerable<AdsGetAllResponce>> GetAll()
    {
        var ads = await _adsRepository.GetAll();
        if (ads is null)
            throw new ArgumentException(string.Format(ValidationMessages.NotFound, nameof(ads)));
        return _mapper.Map<IEnumerable<AdsGetAllResponce>>(ads);
    }

    /// <summary>
    ///     Добавление нового объявления.
    /// </summary>
    /// <param name="entity">Запрос на создание объявления.</param>
    /// <returns>Ответ с информацией о созданном объявлении.</returns>
    public async Task<AdsCreateResponse> Add(AdsCreateRequest entity)
    {
        var adsToAdd = _mapper.Map<Ads>(entity);
        var user = await _userRepository.FindById(entity.UserId);
        if (user is null)
            throw new ArgumentException(string.Format(ValidationMessages.NotFound, nameof(user.Id)));
        if (!await TryToPublic(user.Id))
            throw new ArgumentException(string.Format(ValidationMessages.IsMaxPublic, user.Name));
        await _adsRepository.Add(adsToAdd);
        user.Ads.Add(adsToAdd);
        return _mapper.Map<AdsCreateResponse>(adsToAdd);
    }

    /// <summary>
    ///     Обновление объявления.
    /// </summary>
    /// <param name="entity">Запрос на обновление объявления.</param>
    /// <returns>Результат обновления объявления.</returns>
    public async Task<bool> Update(AdsUpdateRequest entity)
    {
        var ads = await _adsRepository.GetById(entity.Id);
        if (ads is null)
            return false;
        _mapper.Map(entity, ads);
        return await _adsRepository.Update(ads);
    }

    /// <summary>
    ///     Удаление объявления по его идентификатору.
    /// </summary>
    /// <param name="id">Id объявления.</param>
    /// <returns>Результат удаления объявления.</returns>
    public async Task<bool> Delete(Guid id)
    {
        return await _adsRepository.Delete(id);
    }

    /// <summary>
    ///     Поиск объявления по его идентификатору.
    /// </summary>
    /// <param name="id">Id объявления.</param>
    /// <returns>Ответ с информацией о найденном объявлении.</returns>
    public async Task<bool> TryToPublic(Guid id)
    {
        return await _adsRepository.CanUserPublish(id);
    }

    /// <summary>
    ///     Фильтрация объявлений
    /// </summary>
    /// <param name="ascending">по возрастанию или убыванию</param>
    /// <param name="sortBy">Вид сортировки</param>
    /// <param name="text">Для поиска объявления по тексту</param>
    /// <returns></returns>
    public async Task<IEnumerable<AdsFiltrationResponce>> Filtration(bool ascending, SortBy sortBy, string text)
    {
        var ads = await _adsRepository.Filtration(ascending, sortBy, text);
        if (ads.Count() == 0) return null;
        return _mapper.Map<IEnumerable<AdsFiltrationResponce>>(ads);
    }

    /// <summary>
    ///     Поск объявления по Id
    /// </summary>
    /// <param name="id">Id объявления</param>
    /// <returns></returns>
    public async Task<AdsGetAllResponce> FindById(Guid id)
    {
        var ads = await _adsRepository.GetById(id);
        if (ads is null) return null;
        return _mapper.Map<AdsGetAllResponce>(ads);
    }
}