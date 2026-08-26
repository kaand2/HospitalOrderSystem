using FluentValidation;
using HospitalOrderSystem.Application.DTOs.Appointments;

namespace HospitalOrderSystem.Application.Validators.Appointments
{
    public class CancelAppointmentDtoValidator : AbstractValidator<CancelAppointmentDto>
    {
        public CancelAppointmentDtoValidator()
        {
            RuleFor(x => x.CancellationReason)
                .NotEmpty()
                .WithMessage("İptal nedeni zorunludur.")
                .MaximumLength(500)
                .WithMessage("İptal nedeni en fazla 500 karakter olabilir.");
        }
    }
}
