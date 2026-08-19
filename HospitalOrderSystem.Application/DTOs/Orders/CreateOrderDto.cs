using HospitalOrderSystem.Domain.Enums;

namespace HospitalOrderSystem.Application.DTOs.Orders
{
    public class CreateOrderDto
    {
        public string? PatientTcNo { get; set; }
        public string? PatientFirstName { get; set; }
        public string? PatientLastName { get; set; }
        public OrderType OrderType { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
