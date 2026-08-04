using HospitalOrderSystem.Domain.Enums;

namespace HospitalOrderSystem.Domain.Entities
{
    public class OrderAction
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public ActionType ActionType { get; set; }
        public OrderStatus? PreviousStatus { get; set; }
        public OrderStatus? NewStatus { get; set; }
        public string? Result { get; set; }
        public string? Description { get; set; }
        public DateTime ActionDate { get; set; } = DateTime.UtcNow;
        public Order Order { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}
