using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HospitalOrderSystem.Application.DTOs.Users;
using HospitalOrderSystem.Application.Interfaces.Services;
using HospitalOrderSystem.Application.Interfaces.Repositories;
using HospitalOrderSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace HospitalOrderSystem.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<User> _passwordHasher;
        public UserService(IUserRepository userRepository, IMapper mapper, IPasswordHasher<User> passwordHasher)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
        }
        public async Task<List<UserDto>> GetAllAsync()
        {
            List<User> users = await _userRepository.GetAllAsync();
            return _mapper.Map<List<UserDto>>(users);
        }
        public async Task<UserDto> GetByIdAsync(int id)
        {
            User? user = await _userRepository.GetByIdAsync(id);
            if (user is null)
            {
                throw new KeyNotFoundException($"Id değeri {id} olan kullanıcı bulunamadı.");
            }
            return _mapper.Map<UserDto>(user);
        }
        public async Task<UserDto> CreateAsync(CreateUserDto createUserDto)
        {
            string normalizedUsername = createUserDto.Username.Trim();
            bool usernameExists = await _userRepository.UsernameExistsAsync(normalizedUsername);
            if (usernameExists)
            {
                throw new InvalidOperationException("Bu kullanıcı adı ile kayıtlı bir kullanıcı zaten bulunmaktadır.");
            }
            User user = _mapper.Map<User>(createUserDto);
            user.Username = normalizedUsername;
            user.PasswordHash = _passwordHasher.HashPassword(user, createUserDto.Password);
            user.CreatedDate = DateTime.UtcNow;
            user.IsDeleted = false;
            user.UpdatedDate = null;
            user.DeletedDate = null;
            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();
            return _mapper.Map<UserDto>(user);
        }
        public async Task<UserDto> UpdateAsync(int id, UpdateUserDto updateUserDto)
        {
            User? user = await _userRepository.GetByIdAsync(id);
            if (user is null)
            {
                throw new KeyNotFoundException($"Id değeri {id} olan kullanıcı bulunamadı.");
            }
            string normalizedUsername = updateUserDto.Username.Trim();
            bool usernameExists = await _userRepository.UsernameExistsAsync(normalizedUsername, id);
            if (usernameExists)
            {
                throw new InvalidOperationException("Bu kullanıcı adı başka bir kullanıcıya aittir.");
            }
            _mapper.Map(updateUserDto, user);
            user.Username = normalizedUsername;
            user.PasswordHash = _passwordHasher.HashPassword(user, updateUserDto.Password);
            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();
            return _mapper.Map<UserDto>(user);
        }
        public async Task DeleteAsync(int id)
        {
            User? user = await _userRepository.GetByIdAsync(id);
            if (user is null)
            {
                throw new KeyNotFoundException($"Id değeri {id} olan kullanıcı bulunamadı.");
            }
            _userRepository.Delete(user);
            await _userRepository.SaveChangesAsync();
        }
    }
}
