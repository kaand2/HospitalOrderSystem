using HospitalOrderSystem.Application.DTOs.Auth;
using HospitalOrderSystem.Application.DTOs.Users;
using HospitalOrderSystem.Application.Interfaces.Repositories;
using HospitalOrderSystem.Application.Interfaces.Security;
using HospitalOrderSystem.Application.Interfaces.Services;
using HospitalOrderSystem.Domain.Entities;
using HospitalOrderSystem.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace HospitalOrderSystem.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AuthService(
            IUserRepository userRepository,
            IJwtTokenGenerator jwtTokenGenerator,
            IPasswordHasher<User> passwordHasher)
        {
            _userRepository = userRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
            _passwordHasher = passwordHasher;
        }

        public async Task<LoginResponseDto?> LoginAsync(LoginDto dto)
        {
            var user = await _userRepository.GetByUsernameAsync(dto.Username);

            if (user is null || user.IsDeleted)
                return null;

            var passwordResult = _passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                dto.Password);

            if (passwordResult == PasswordVerificationResult.Failed)
            {
                return null;
            }

            var token = _jwtTokenGenerator.GenerateToken(user);

            return new LoginResponseDto
            {
                Token = token,
                Expiration = DateTime.UtcNow.AddMinutes(60)
            };
        }
        public async Task<UserDto> CreateInitialAdminAsync(CreateInitialAdminDto dto)
        {
            var adminExists = await _userRepository.AdminExistsAsync();
            if (adminExists)
            {
                throw new InvalidOperationException("Initial admin zaten oluşturulmuş.");
            }
            string normalizedUsername = dto.Username.Trim();
            bool usernameExists = await _userRepository.UsernameExistsAsync(normalizedUsername);
            if (usernameExists)
            {
                throw new InvalidOperationException("Bu kullanıcı adı alınmıştır.");
            }
            var user = new User
            {
                Username = normalizedUsername,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Role = UserRole.Admin,
                IsDeleted = false,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = null,
                DeletedDate = null
            };
            user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);
            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();
            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role,
                IsDeleted = user.IsDeleted,
                CreatedDate = user.CreatedDate
            };
        }
    }
}
