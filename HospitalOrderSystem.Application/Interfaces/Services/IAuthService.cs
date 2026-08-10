using HospitalOrderSystem.Application.DTOs.Auth;
using HospitalOrderSystem.Application.DTOs.Users;

namespace HospitalOrderSystem.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<LoginResponseDto?> LoginAsync(LoginDto dto);
        Task<UserDto> CreateInitialAdminAsync(CreateInitialAdminDto dto);
    }
}
