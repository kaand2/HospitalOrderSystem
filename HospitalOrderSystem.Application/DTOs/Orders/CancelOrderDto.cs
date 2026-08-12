using System.ComponentModel.DataAnnotations;

namespace HospitalOrderSystem.Application.DTOs.Orders
{
    public class CancelOrderDto
    {
        public string CancellationReason { get; set; } = string.Empty;
    }
}
