using HospitalOrderSystem.Domain.Entities;

namespace HospitalOrderSystem.Application.Interfaces.Repositories
{
    public interface IOrderActionRepository
    {
        Task<List<OrderAction>> GetAllAsync();
        Task<List<OrderAction>> GetByOrderIdAsync(int orderId);
        Task<OrderAction?> GetByIdAsync(int id);
        Task AddAsync(OrderAction orderAction);
        Task<int> SaveChangesAsync();
    }
}
