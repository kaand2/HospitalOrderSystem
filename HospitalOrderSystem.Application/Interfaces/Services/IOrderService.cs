using HospitalOrderSystem.Application.DTOs.Orders;

namespace HospitalOrderSystem.Application.Interfaces.Services
{
    public interface IOrderService
    {
        Task<List<OrderDto>> GetAllAsync();
        Task<OrderDto> GetByIdAsync(int id);
        Task<OrderDto> CreateAsync(CreateOrderDto createOrderDto);
        Task<OrderDto> UpdateAsync(int id, UpdateOrderDto updateOrderDto);
        Task DeleteAsync(int id);
    }
}
