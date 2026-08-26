using HospitalOrderSystem.Application.Interfaces.Repositories;
using HospitalOrderSystem.Domain.Entities;
using HospitalOrderSystem.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace HospitalOrderSystem.Persistence.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly ProjectDbContext _context;

        public AppointmentRepository(ProjectDbContext context)
        {
            _context = context;
        }

        public async Task<List<Appointment>> GetAllAsync()
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => !a.IsDeleted)
                .ToListAsync();
        }

        public async Task<Appointment?> GetByIdAsync(int id)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted);
        }

        public async Task<List<Appointment>> SearchAsync(string? patientName, string? doctorName, DateTime? date)
        {
            var query = _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Where(a => !a.IsDeleted);

            if (!string.IsNullOrWhiteSpace(patientName))
                query = query.Where(a =>
                    a.Patient.FirstName.ToLower().Contains(patientName.Trim().ToLower()) ||
                    a.Patient.LastName.ToLower().Contains(patientName.Trim().ToLower()));

            if (!string.IsNullOrWhiteSpace(doctorName))
                query = query.Where(a =>
                    a.Doctor.FirstName.ToLower().Contains(doctorName.Trim().ToLower()) ||
                    a.Doctor.LastName.ToLower().Contains(doctorName.Trim().ToLower()));

            if (date.HasValue)
                query = query.Where(a => a.AppointmentDate.Date == date.Value.Date);

            return await query.ToListAsync();
        }

        public async Task<List<Appointment>> GetByDoctorAndDateAsync(int doctorId, DateTime date)
        {
            return await _context.Appointments
                .Where(a => !a.IsDeleted &&
                            a.DoctorId == doctorId &&
                            a.AppointmentDate.Date == date.Date &&
                            !a.IsCancelled)
                .ToListAsync();
        }

        public async Task AddAsync(Appointment appointment)
        {
            await _context.Appointments.AddAsync(appointment);
        }

        public void Update(Appointment appointment)
        {
            appointment.UpdatedDate = DateTime.UtcNow;
            _context.Appointments.Update(appointment);
        }

        public void Delete(Appointment appointment)
        {
            appointment.IsDeleted = true;
            appointment.DeletedDate = DateTime.UtcNow;
            appointment.UpdatedDate = DateTime.UtcNow;
            _context.Appointments.Update(appointment);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
