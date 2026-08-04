using FluentValidation;
using HospitalOrderSystem.Application.DTOs.Patients;

namespace HospitalOrderSystem.Application.Validators.Patients
{
    public class CreatePatientDtoValidator : AbstractValidator<CreatePatientDto>
    {
        public CreatePatientDtoValidator()
        {
            RuleFor(patient => patient.TcNo)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("TC Kimlik Numarası zorunludur.")
                .Matches(@"^\d{11}$")
                .WithMessage("TC Kimlik Numarası 11 rakamdan oluşmalıdır.");
            RuleFor(patient => patient.FirstName)
                .NotEmpty()
                .WithMessage("Hasta adı zorunludur.");
            RuleFor(patient => patient.LastName)
                .NotEmpty()
                .WithMessage("Hasta soyadı zorunludur.");
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