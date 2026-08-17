using HospitalOrderSystem.Application.DTOs.Auth;
using HospitalOrderSystem.Application.DTOs.Users;

namespace HospitalOrderSystem.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<LoginResponseDto?> LoginAsync(LoginDto dto);
        Task SeedInitialAdminAsync(string username, string password, string firstName, string lastName);
    }
}
