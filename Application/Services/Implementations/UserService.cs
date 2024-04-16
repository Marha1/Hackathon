using Application.Dtos.UserDto;
using Application.Dtos.UserDto.Request;
using Application.Services.Interfaces;
using AutoMapper;
using Infrastructure.DAL.Interfaces;
using Domain.Enities;

namespace Application.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository<User> _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository<User> userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserGetByIdResponse> FindById(Guid id)
        {
            var user = await _userRepository.FindById(id);
            return _mapper.Map<UserGetByIdResponse>(user);
        }

        public async Task<UserGetAllResponse> GetAll()
        {
            var users = await _userRepository.GetAll();
            var userDtOs = users.Select(user => _mapper.Map<BaseUserDto>(user));

            var response = new UserGetAllResponse
            {
                Users = userDtOs.ToList()
            };

            return response;
        }

        public async Task<UserCreateResponse> Add(UserCreateRequest entity)
        {
            var userToAdd = _mapper.Map<User>(entity);
            await _userRepository.Add(userToAdd);
            var addedUser = await _userRepository.FindById(userToAdd.Id);
            return _mapper.Map<UserCreateResponse>(addedUser);
        }
        
        public async Task<bool> Update(UserUpdateRequest entity)
        {
            var users = _mapper.Map<User>(entity);
            return await _userRepository.Update(users);
        }

        public async Task<bool> Delete(Guid id)
        {
            return await _userRepository.Delete(id);
        }
    }
}