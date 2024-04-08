using System;
using System.Linq;
using Application.Dtos;
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

        var userDTOs = users.Select(user => new BaseUserDto
        {
            Id = user.Id,
            Name = user.Name,
            isAdmins = user.Admin
        });

        var response = new UserGetAllResponse
        {
            Users = userDTOs
        };

        return response;
    }

    public UserCreateResponse Add(User entity)
    {
        var userToAdd = _mapper.Map<User>(entity);
        _userRepository.Add(userToAdd);
        var addedUser = _userRepository.FindById(userToAdd.Id);
        return _mapper.Map<UserCreateResponse>(addedUser);
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