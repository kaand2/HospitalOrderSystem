using HospitalOrderSystem.Application.DTOs.Auth;
using HospitalOrderSystem.Application.Interfaces.Repositories;
using HospitalOrderSystem.Application.Interfaces.Security;
using HospitalOrderSystem.Application.Interfaces.Services;
using HospitalOrderSystem.Domain.Entities;
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
    }
}
