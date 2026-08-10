using HospitalOrderSystem.Domain.Entities;

namespace HospitalOrderSystem.Application.Interfaces.Repositories
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetAllAsync();
        Task<Order?> GetByIdAsync(int id);
        Task AddAsync(Order order);
        void Update(Order order);
        void Delete(Order order);
        Task<int> SaveChangesAsync();
    }
}
