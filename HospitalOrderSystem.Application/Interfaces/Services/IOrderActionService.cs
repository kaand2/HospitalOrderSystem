using HospitalOrderSystem.Application.DTOs.OrderActions;

namespace HospitalOrderSystem.Application.Interfaces.Services
{
    public interface IOrderActionService
    {
        Task<List<OrderActionDto>> GetAllAsync();
        Task<List<OrderActionDto>> GetByOrderIdAsync(int orderId);
        Task<OrderActionDto> GetByIdAsync(int id);
        Task<OrderActionDto> CreateAsync(CreateOrderActionDto createDto);
    }
}
