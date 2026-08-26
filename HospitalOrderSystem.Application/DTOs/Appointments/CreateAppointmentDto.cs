namespace HospitalOrderSystem.Application.DTOs.Appointments
{
    public class CreateAppointmentDto
    {
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string? Reason { get; set; }
        public string? Notes { get; set; }
    }
}
