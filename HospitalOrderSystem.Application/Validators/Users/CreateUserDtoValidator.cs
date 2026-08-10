using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using HospitalOrderSystem.Application.DTOs.Users;

namespace HospitalOrderSystem.Application.Validators.Users
{
    public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
    {
        public CreateUserDtoValidator()
        {
            RuleFor(user => user.Username)
                .NotEmpty()
                .WithMessage("Kullanıcı adı zorunludur.");
            RuleFor(user => user.Password)
                .NotEmpty()
                .WithMessage("Şifre zorunludur.")
                .MinimumLength(6)
                .WithMessage("Şifre en az 6 karakter olmalıdır.");
            RuleFor(user => user.FirstName)
                .NotEmpty()
                .WithMessage("Ad zorunludur.");
            RuleFor(user => user.LastName)
                .NotEmpty()
                .WithMessage("Soyad zorunludur.");
            RuleFor(user => user.Role)
                .IsInEnum()
                .WithMessage("Geçerli bir rol seçilmelidir.");
        }
    }
}
