using FluentValidation;
using HospitalOrderSystem.Application.DTOs.Appointments;

namespace HospitalOrderSystem.Application.Validators.Appointments
{
    public class CreateAppointmentDtoValidator : AbstractValidator<CreateAppointmentDto>
    {
        public CreateAppointmentDtoValidator()
        {
            RuleFor(x => x.PatientId)
                .GreaterThan(0)
                .WithMessage("Geçerli bir hasta seçiniz.");

            RuleFor(x => x.DoctorId)
                .GreaterThan(0)
                .WithMessage("Geçerli bir doktor seçiniz.");

            RuleFor(x => x.AppointmentDate)
                .GreaterThan(DateTime.UtcNow)
                .WithMessage("Randevu tarihi gelecekte olmalı.");

            RuleFor(x => x.Reason)
                .MaximumLength(500)
                .WithMessage("Randevu nedeni en fazla 500 karakter olmalı.")
                .When(x => x.Reason != null);
        }
    }
}
