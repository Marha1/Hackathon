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
    private readonly IAdsRepository<Ads> _adsRepository;
    private readonly IMapper _mapper;
    private readonly IUserRepository<User> _userRepository;

    public AdsService(IAdsRepository<Ads> adsRepository, IMapper mapper, IUserRepository<User> userRepository)
    {
        _adsRepository = adsRepository;
        _mapper = mapper;
        _userRepository = userRepository;
    }

    /// <summary>
    ///     Получает все объявления-
    /// </summary>
    /// <returns>Ответ,содержащий все объявления</returns>
    public async Task<IEnumerable<AdsGetAllResponce>> GetAll()
    {
        var ads = await _adsRepository.GetAll();
        var adDtos = ads.Select(ad => _mapper.Map<AdsGetAllResponce>(ad));
        return adDtos;
    }

    /// <summary>
    ///     Добавляет новое объявление.
    /// </summary>
    /// <param name="entity">Запрос на создание объявления.</param>
    /// <returns>Ответ, содержащий информацию о добавленном объявлении.</returns>
    public async Task<AdsCreateResponse> Add(AdsCreateRequest entity)
    {
        var adsToAdd = _mapper.Map<Ads>(entity);
        var user = await _userRepository.FindById(entity.UserId);
        if (user is null) return null;
        await _adsRepository.Add(adsToAdd);

        user.Ads.Add(adsToAdd);

        return _mapper.Map<AdsCreateResponse>(adsToAdd);
    }

    /// <summary>
    ///     Обновление объявления.
    /// </summary>
    /// <param name="entity">Запрос на обновление объявления.</param>
    /// <returns>Ответ, содержащий true(успешно обновлено) или false(ошибка при обновлении) .</returns>
    public async Task<bool> Update(AdsUpdateRequest entity)
    {
        var ads = await _adsRepository.GetById(entity.Id);

        if (ads is null) return false;

        _mapper.Map(entity, ads);
        return await _adsRepository.Update(ads);
    }

    /// <summary>
    ///     Удаление объявления.
    /// </summary>
    /// <param name="id">Запрос на удаление объявления.</param>
    /// <returns>Ответ, содержащий true(успешно удалено) или false(ошибка при удалении) .</returns>
    public async Task<bool> Delete(Guid id)
    {
        return await _adsRepository.Delete(id);
    }

    /// <summary>
    ///     Проверка возможности размещения объявления пользователем.
    /// </summary>
    /// <param name="id">Id пользователя .</param>
    /// <returns>
    ///     Ответ, содержащий true(возможно размещение нового обьявления) или false(ошибка при добавлении нового
    ///     объявления) .
    /// </returns>
    public bool TryToPublic(Guid id)
    {
        return _adsRepository.CanUserPublish(id);
    }

    /// <summary>
    ///     Поиск объявления по тексту
    /// </summary>
    /// <param name="text"></param>
    /// <returns>Ответ,содержащий информацию о найденом объявлении</returns>
    public async Task<IEnumerable<AdsGetByTextResponce>> FindByText(string text)
    {
        var ads = await _adsRepository.GetAll();
        if (ads is null) return null;
        var adsWhere = ads.Where(ad => ad.Text == text).ToList();
        var adsDtoList = adsWhere.Select(ad => _mapper.Map<AdsGetByTextResponce>(ad)).ToList();
        return adsDtoList;
    }

    /// <summary>
    ///     Фильтр объявлений
    /// </summary>
    /// <param name="ascending">Флаг, определяющий порядок сортировки (по возрастанию или убыванию)</param>
    /// <returns>Список отфильтрованных и отсортированных объявлений</returns>
    public async Task<IEnumerable<AdsFiltrationResponce>> Filtration(bool ascending)
    {
        var ads = await _adsRepository.GetAll();
        if (ads is null) return null;
        var adsOrder = ascending ? ads.OrderBy(n => n.Rating) : ads.OrderByDescending(n => n.Rating);
        var adsDtoList = adsOrder.Select(ad => _mapper.Map<AdsFiltrationResponce>(ad)).ToList();
        return adsDtoList;
    }
}