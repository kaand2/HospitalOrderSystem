using HospitalOrderSystem.Domain.Enums;
using System;
using System.Collections.Generic;

namespace HospitalOrderSystem.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int CreatedByUserId { get; set; }
        public OrderType OrderType { get; set; }
        public OrderStatus Status { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public DateTime? CancelledDate { get; set; }
        public string? CancellationReason { get; set; }
        public bool IsDeleted { get; set; } = false;
        public Patient Patient { get; set; } = null!;
        public User CreatedByUser { get; set; } = null!;
        public ICollection<OrderAction> Actions { get; set; } = new List<OrderAction>();
    }
}
