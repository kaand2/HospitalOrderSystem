using HospitalOrderSystem.Domain.Enums;

namespace HospitalOrderSystem.Application.DTOs.Appointments
{
    public class UpdateAppointmentDto
    {
        public DateTime AppointmentDate { get; set; }
        public AppointmentStatus Status { get; set; }
        public string? Reason { get; set; }
        public string? Notes { get; set; }
    }
}
