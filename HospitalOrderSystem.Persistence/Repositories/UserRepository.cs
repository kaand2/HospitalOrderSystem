using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospitalOrderSystem.Application.Interfaces.Repositories;
using HospitalOrderSystem.Domain.Entities;
using HospitalOrderSystem.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace HospitalOrderSystem.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ProjectDbContext _context;
    public UserRepository(ProjectDbContext context)
    {
        _context = context;
    }
    public async Task<List<User>> GetAllAsync()
    {
        return await _context.Users
            .Where(user => !user.IsDeleted)
            .ToListAsync();
    }
    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users
            .FirstOrDefaultAsync(user =>
                user.Id == id &&
                !user.IsDeleted);
    }
    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _context.Users
            .FirstOrDefaultAsync(user =>
                user.Username == username &&
                !user.IsDeleted);
    }
    public async Task<bool> UsernameExistsAsync(string username, int? excludedUserId = null)
    {
        return await _context.Users.AnyAsync(user =>
            user.Username == username && (!excludedUserId.HasValue || user.Id != excludedUserId.Value));
    }
    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
    }
    public void Update(User user)
    {
        user.UpdatedDate = DateTime.UtcNow;
        _context.Users.Update(user);
    }
    public void Delete(User user)
    {
        user.IsDeleted = true;
        user.DeletedDate = DateTime.UtcNow;
        user.UpdatedDate = DateTime.UtcNow;

        _context.Users.Update(user);
    }
    public async Task<bool> AdminExistsAsync()
    {
        return await _context.Users.AnyAsync(user => user.Role == Domain.Enums.UserRole.Admin && !user.IsDeleted);
    }
    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
