using FluentValidation;
using HospitalOrderSystem.Application.DTOs.Patients;

namespace HospitalOrderSystem.Application.Validators.Patients
{
    public class UpdatePatientDtoValidator : AbstractValidator<UpdatePatientDto>
    {
        public UpdatePatientDtoValidator()
        {
            RuleFor(patient => patient.TcNo)
                .NotEmpty()
                .WithMessage("TC Kimlik numarası zorunludur.")
                .Matches(@"^\d{11}$")
                .WithMessage("TC Kimlik numarası 11 rakamdan oluşmalıdır.");
            RuleFor(patient => patient.FirstName)
                .NotEmpty()
                .WithMessage("Hasta adı zorunludur.");
            RuleFor(patient => patient.LastName)
                .NotEmpty()
                .WithMessage("Hasta soyadı zorunludr.");
            RuleFor(patient => patient.BirthDate)
                .NotEmpty()
                .WithMessage("Doğum tarihi zorunludur.")
                .Must(birthDate => birthDate.Date <= DateTime.UtcNow.Date)
                .WithMessage("Doğum tarihi gelecekte olamaz.");
            RuleFor(patient => patient.Gender)
                .IsInEnum()
                .WithMessage("Geçerli bir cinsiyet seçilmelidir.");
            RuleFor(patient => patient.Email)
                .EmailAddress()
                .When(patient => !string.IsNullOrWhiteSpace(patient.Email))
                .WithMessage("Geçerli bir e-posta adresi girilmelidir.");
            RuleFor(patient => patient.InsuranceType)
                .IsInEnum()
                .WithMessage("Geçerli bir sigorta türü seçilmelidir.");
        }
    }
}