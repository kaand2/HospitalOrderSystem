using HospitalOrderSystem.Domain.Entities;

namespace HospitalOrderSystem.Application.Interfaces.Repositories
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetAllAsync();
        Task<Order?> GetByIdAsync(int id);
        Task<List<Order>> SearchAsync(
            string? patientFirstName,
            string? patientLastName,
            string? patientTcNo,
            string? doctorFirstName,
            string? doctorLastName);
        Task<List<Order>> GetByDoctorAndDateAsync(int doctorId, DateTime date);
        Task AddAsync(Order order);
        void Update(Order order);
        void Delete(Order order);
        Task<int> SaveChangesAsync();
    }
}
