using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospitalOrderSystem.Domain.Entities;

namespace HospitalOrderSystem.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByUsernameAsync(string username);
        Task<bool> UsernameExistsAsync(string username, int? excludedUserId = null);
        Task AddAsync(User user);
        void Update(User user);
        void Delete(User user);
        Task<int> SaveChangesAsync();
    }
}
