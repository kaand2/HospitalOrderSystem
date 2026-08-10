using HospitalOrderSystem.Domain.Enums;

namespace HospitalOrderSystem.Application.DTOs.Orders
{
    public class UpdateOrderDto
    {
        public OrderType OrderType { get; set; }
        public OrderStatus Status { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? CancellationReason { get; set; }
    }
}
