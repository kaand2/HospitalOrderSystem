using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospitalOrderSystem.Application.DTOs.Users;

namespace HospitalOrderSystem.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAllAsync(int? role = null);
        Task<UserDto> GetByIdAsync(int id);
        Task<List<UserDto>> SearchAsync(string? firstName, string? lastName);
        Task<List<UserDto>> GetDoctorsAsync();
        Task<List<UserDto>> SearchDoctorsAsync(string? firstName, string? lastName);
        Task<UserDto> CreateAsync(CreateUserDto createUserDto);
        Task<UserDto> UpdateAsync(int id, UpdateUserDto updateUserDto);
        Task DeleteAsync(int id);
    }
}
