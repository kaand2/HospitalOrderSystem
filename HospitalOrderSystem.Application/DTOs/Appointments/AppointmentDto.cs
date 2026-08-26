using HospitalOrderSystem.Domain.Enums;

namespace HospitalOrderSystem.Application.DTOs.Appointments
{
    public class AppointmentDto
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public string PatientFirstName { get; set; } = string.Empty;
        public string PatientLastName { get; set; } = string.Empty;
        public string PatientTcNo { get; set; } = string.Empty;
        public int DoctorId { get; set; }
        public string DoctorFirstName { get; set; } = string.Empty;
        public string DoctorLastName { get; set; } = string.Empty;
        public DateTime AppointmentDate { get; set; }
        public AppointmentStatus Status { get; set; }
        public string? Reason { get; set; }
        public string? Notes { get; set; }
        public bool IsCancelled { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? CancelledDate { get; set; }
        public string? CancellationReason { get; set; }
    }
}
