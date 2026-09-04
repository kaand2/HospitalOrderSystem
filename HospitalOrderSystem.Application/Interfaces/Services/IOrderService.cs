using HospitalOrderSystem.Application.DTOs.Orders;

namespace HospitalOrderSystem.Application.Interfaces.Services
{
    public interface IOrderService
    {
        Task<List<OrderDto>> GetAllAsync(string userRole);
        Task<OrderDto> GetByIdAsync(int id, string userRole);
        Task<List<OrderDto>> SearchAsync(
            string userRole,
            string? patientFirstName,
            string? patientLastName,
            string? patientTcNo,
            string? doctorFirstName,
            string? doctorLastName);
        Task<OrderDto> CreateAsync(int createdByUserId, CreateOrderDto createOrderDto);
        Task<OrderDto> UpdateAsync(int id, UpdateOrderDto updateOrderDto);
        Task<OrderDto> AssignUserAsync(int orderId, AssignOrderDto assignDto, string userRole);
        Task<OrderDto> CancelAsync(int id, CancelOrderDto cancelOrderDto);
        Task DeleteAsync(int id);
    }
}
