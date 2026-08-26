using HospitalOrderSystem.Domain.Entities;

namespace HospitalOrderSystem.Application.Interfaces.Repositories
{
    public interface IAppointmentRepository
    {
        Task<List<Appointment>> GetAllAsync();
        Task<Appointment?> GetByIdAsync(int id);
        Task<List<Appointment>> SearchAsync(string? patientName, string? doctorName, DateTime? date);
        Task<List<Appointment>> GetByDoctorAndDateAsync(int doctorId, DateTime date);
        Task AddAsync(Appointment appointment);
        void Update(Appointment appointment);
        void Delete(Appointment appointment);
        Task<int> SaveChangesAsync();
    }
}
