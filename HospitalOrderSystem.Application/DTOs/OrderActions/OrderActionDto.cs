using HospitalOrderSystem.Domain.Enums;

namespace HospitalOrderSystem.Application.DTOs.OrderActions
{
    public class OrderActionDto
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public ActionType ActionType { get; set; }
        public OrderStatus? PreviousStatus { get; set; }
        public OrderStatus? NewStatus { get; set; }
        public string? Result { get; set; }
        public string? Description { get; set; }
        public DateTime ActionDate { get; set; }
    }
}
