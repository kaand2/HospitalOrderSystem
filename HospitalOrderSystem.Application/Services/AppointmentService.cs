using AutoMapper;
using HospitalOrderSystem.Application.DTOs.Appointments;
using HospitalOrderSystem.Application.Interfaces.Repositories;
using HospitalOrderSystem.Application.Interfaces.Services;
using HospitalOrderSystem.Domain.Entities;
using HospitalOrderSystem.Domain.Enums;

namespace HospitalOrderSystem.Application.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IMapper _mapper;

        public AppointmentService(IAppointmentRepository appointmentRepository, IMapper mapper)
        {
            _appointmentRepository = appointmentRepository;
            _mapper = mapper;
        }

        public async Task<List<AppointmentDto>> GetAllAsync()
        {
            List<Appointment> appointments = await _appointmentRepository.GetAllAsync();
            return _mapper.Map<List<AppointmentDto>>(appointments);
        }

        public async Task<AppointmentDto> GetByIdAsync(int id)
        {
            Appointment? appointment = await _appointmentRepository.GetByIdAsync(id);
            if (appointment is null)
            {
                throw new KeyNotFoundException($"Id değeri {id} olan randevu bulunamadı.");
            }
            return _mapper.Map<AppointmentDto>(appointment);
        }

        public async Task<List<AppointmentDto>> SearchAsync(string? patientName, string? doctorName, DateTime? date)
        {
            List<Appointment> appointments = await _appointmentRepository.SearchAsync(patientName, doctorName, date);
            return _mapper.Map<List<AppointmentDto>>(appointments);
        }

        public async Task<List<string>> GetAvailableTimeSlotsAsync(int doctorId, DateTime date)
        {
            var appointments = await _appointmentRepository.GetByDoctorAndDateAsync(doctorId, date);

            var timeSlots = new List<string>();
            var startTime = new TimeSpan(9, 0, 0);
            var endTime = new TimeSpan(17, 0, 0);
            var interval = TimeSpan.FromMinutes(15);

            for (var time = startTime; time < endTime; time += interval)
            {
                var slotDateTime = date.Date + time;

                bool isTaken = appointments.Any(a =>
                    a.AppointmentDate.TimeOfDay == time);

                if (!isTaken && slotDateTime > DateTime.UtcNow)
                {
                    timeSlots.Add(time.ToString(@"hh\:mm"));
                }
            }

            return timeSlots;
        }

        public async Task<AppointmentDto> CreateAsync(CreateAppointmentDto dto)
        {
            Appointment appointment = _mapper.Map<Appointment>(dto);
            appointment.Status = AppointmentStatus.Scheduled;
            appointment.IsCancelled = false;
            appointment.CreatedDate = DateTime.UtcNow;
            appointment.IsDeleted = false;
            appointment.UpdatedDate = null;
            appointment.CancelledDate = null;
            appointment.DeletedDate = null;

            await _appointmentRepository.AddAsync(appointment);
            await _appointmentRepository.SaveChangesAsync();

            Appointment? created = await _appointmentRepository.GetByIdAsync(appointment.Id);
            return _mapper.Map<AppointmentDto>(created!);
        }

        public async Task<AppointmentDto> UpdateAsync(int id, UpdateAppointmentDto dto)
        {
            Appointment? appointment = await _appointmentRepository.GetByIdAsync(id);
            if (appointment is null)
            {
                throw new KeyNotFoundException($"Id değeri {id} olan randevu bulunamadı.");
            }

            _mapper.Map(dto, appointment);
            _appointmentRepository.Update(appointment);
            await _appointmentRepository.SaveChangesAsync();

            return _mapper.Map<AppointmentDto>(appointment);
        }

        public async Task<AppointmentDto> CancelAsync(int id, CancelAppointmentDto dto)
        {
            Appointment? appointment = await _appointmentRepository.GetByIdAsync(id);
            if (appointment is null)
            {
                throw new KeyNotFoundException($"Id değeri {id} olan randevu bulunamadı.");
            }

            if (appointment.IsCancelled)
            {
                throw new InvalidOperationException("Bu randevu zaten iptal edilmiş.");
            }

            appointment.Status = AppointmentStatus.Cancelled;
            appointment.IsCancelled = true;
            appointment.CancellationReason = dto.CancellationReason;
            appointment.CancelledDate = DateTime.UtcNow;

            _appointmentRepository.Update(appointment);
            await _appointmentRepository.SaveChangesAsync();

            return _mapper.Map<AppointmentDto>(appointment);
        }

        public async Task DeleteAsync(int id)
        {
            Appointment? appointment = await _appointmentRepository.GetByIdAsync(id);
            if (appointment is null)
            {
                throw new KeyNotFoundException($"Id değeri {id} olan randevu bulunamadı.");
            }

            _appointmentRepository.Delete(appointment);
            await _appointmentRepository.SaveChangesAsync();
        }
    }
}
