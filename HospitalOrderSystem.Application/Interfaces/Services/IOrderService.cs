using HospitalOrderSystem.Application.DTOs.Orders;

namespace HospitalOrderSystem.Application.Interfaces.Services
{
    public interface IOrderService
    {
        Task<List<OrderDto>> GetAllAsync(string userRole);
        Task<OrderDto> GetByIdAsync(int id, string userRole);
        Task<OrderDto> CreateAsync(CreateOrderDto createOrderDto);
        Task<OrderDto> UpdateAsync(int id, UpdateOrderDto updateOrderDto);
        Task<OrderDto> CancelAsync(int id, CancelOrderDto cancelOrderDto);
        Task DeleteAsync(int id);
    }
}
