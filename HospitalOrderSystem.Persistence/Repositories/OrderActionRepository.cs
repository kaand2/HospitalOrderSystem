using HospitalOrderSystem.Application.Interfaces.Repositories;
using HospitalOrderSystem.Domain.Entities;
using HospitalOrderSystem.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace HospitalOrderSystem.Persistence.Repositories
{
    public class OrderActionRepository : IOrderActionRepository
    {
        private readonly ProjectDbContext _context;
        public OrderActionRepository(ProjectDbContext context)
        {
            _context = context;
        }

        public async Task<List<OrderAction>> GetAllAsync()
        {
            return await _context.OrderActions.ToListAsync();
        }

        public async Task<List<OrderAction>> GetByOrderIdAsync(int orderId)
        {
            return await _context.OrderActions
                .Where(action => action.OrderId == orderId)
                .OrderByDescending(action => action.ActionDate)
                .ToListAsync();
        }

        public async Task<OrderAction?> GetByIdAsync(int id)
        {
            return await _context.OrderActions
                .FirstOrDefaultAsync(action => action.Id == id);
        }

        public async Task AddAsync(OrderAction orderAction)
        {
            await _context.OrderActions.AddAsync(orderAction);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
