using HospitalOrderSystem.Application.DTOs.Appointments;

namespace HospitalOrderSystem.Application.Interfaces.Services
{
    public interface IAppointmentService
    {
        Task<List<AppointmentDto>> GetAllAsync();
        Task<AppointmentDto> GetByIdAsync(int id);
        Task<List<AppointmentDto>> SearchAsync(string? patientName, string? doctorName, DateTime? date);
        Task<List<string>> GetAvailableTimeSlotsAsync(int doctorId, DateTime date);
        Task<AppointmentDto> CreateAsync(CreateAppointmentDto dto);
        Task<AppointmentDto> UpdateAsync(int id, UpdateAppointmentDto dto);
        Task<AppointmentDto> CancelAsync(int id, CancelAppointmentDto dto);
        Task DeleteAsync(int id);
    }
}
