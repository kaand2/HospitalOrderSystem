using FluentValidation;
using HospitalOrderSystem.Application.DTOs.Appointments;

namespace HospitalOrderSystem.Application.Validators.Appointments
{
    public class UpdateAppointmentDtoValidator : AbstractValidator<UpdateAppointmentDto>
    {
        public UpdateAppointmentDtoValidator()
        {
            RuleFor(x => x.AppointmentDate)
                .GreaterThan(DateTime.UtcNow)
                .WithMessage("Randevu tarihi gelecekte olmalıdır.");

            RuleFor(x => x.Status)
                .IsInEnum()
                .WithMessage("Geçerli bir randevu durumu seçilmelidir.");
        }
    }
}
