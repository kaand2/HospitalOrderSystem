using HospitalOrderSystem.Application.DTOs.Auth;

namespace HospitalOrderSystem.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<LoginResponseDto?> LoginAsync(LoginDto dto);
    }
}
