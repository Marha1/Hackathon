using Application.Dtos.UserDto.Request;
using Application.Dtos.UserDto.Responce;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Enities;
using Domain.Primitives;
using Infrastructure.DAL.Interfaces;

namespace Application.Services.Implementations;

/// <summary>
///     Реализация сервиса для работы с пользователями.
/// </summary>
public class UserService : IUserService
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    /// <summary>
    ///     Находит пользователя по его идентификатору.
    /// </summary>
    /// <param name="id">Id пользователя.</param>
    /// <returns>Ответ с информацией о пользователе.</returns>
    public async Task<UserGetByIdResponse> FindById(Guid id)
    {
        var user = await _userRepository.FindById(id);
        if (user is null)
            throw new ArgumentException(string.Format(ValidationMessages.NotFound, nameof(user.Id)));
        return _mapper.Map<UserGetByIdResponse>(user);
    }

    /// <summary>
    ///     Получает всех пользователей.
    /// </summary>
    /// <returns>Ответ с информацией о всех пользователях.</returns>
    public async Task<IEnumerable<UserGetAllResponse>> GetAll()
    {
        var users = await _userRepository.GetAll();
        return _mapper.Map<IEnumerable<UserGetAllResponse>>(users);
    }

    /// <summary>
    ///     Добавляет нового пользователя.
    /// </summary>
    /// <param name="entity">Запрос на создание пользователя.</param>
    /// <returns>Ответ с информацией о созданном пользователе.</returns>
    public async Task<UserCreateResponse> Add(UserCreateRequest entity)
    {
        var userToAdd = _mapper.Map<User>(entity);
        await _userRepository.Add(userToAdd);
        var addedUser = await _userRepository.FindById(userToAdd.Id);
        return _mapper.Map<UserCreateResponse>(addedUser);
    }

    /// <summary>
    ///     Обновляет информацию о пользователе.
    /// </summary>
    /// <param name="entity">Запрос на обновление информации о пользователе.</param>
    /// <returns>Результат обновления информации о пользователе.</returns>
    public async Task<bool> Update(UserUpdateRequest entity)
    {
        var user = _mapper.Map<User>(entity);
        return await _userRepository.Update(user);
    }

    /// <summary>
    ///     Удаляет пользователя по его идентификатору.
    /// </summary>
    /// <param name="id">Id пользователя.</param>
    /// <returns>Результат удаления пользователя.</returns>
    public async Task<bool> Delete(Guid id)
    {
        return await _userRepository.Delete(id);
    }
}