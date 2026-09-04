using HospitalOrderSystem.Domain.Enums;

namespace HospitalOrderSystem.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public ICollection<Order> CreatedOrders { get; set; } = new List<Order>();
        public ICollection<OrderAction> Actions { get; set; } = new List<OrderAction>();
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public int? CurrentOrderId { get; set; }
        public Order? CurrentOrder { get; set; }
    }
}
