using Application.Dtos.UserDto;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Enities;
using Infrastrucure.DAL.Interfaces;

namespace Application.Services.Implementations;

public class UserService:IUserService
{
    private readonly IUserRepository<User> _userRepository;
    private readonly IMapper _mapper;

    public UserService(IUserRepository<User> userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }
    public UserGetByIdResponse FindById(Guid id)
    {
        var user =_userRepository.FindById(id);
        return _mapper.Map<UserGetByIdResponse>(user);
    }

    

    public UserGetAllResponse GetAll()
    {
        var users = _userRepository.GetAll();
         return _mapper.Map<UserGetAllResponse>(users);
    }

    public UserCreateResponse Add(User entity)
    {
        var users = _mapper.Map<User>(entity);
        _userRepository.Add(users);
        return _mapper.Map<UserCreateResponse>(users);
    }

    

    public bool Update(User entity)
    {
        var users = _mapper.Map<User>(entity);
        
        return  _userRepository.Update(users);
    }


    public bool Delete(Guid id)
    {
        return _userRepository.Delete(id);
    }
}