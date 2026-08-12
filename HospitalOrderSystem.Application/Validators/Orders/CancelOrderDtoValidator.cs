using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using HospitalOrderSystem.Application.DTOs.Orders;

namespace HospitalOrderSystem.Application.Validators.Orders
{
    public class CancelOrderDtoValidator : AbstractValidator<CancelOrderDto>
    {
        public CancelOrderDtoValidator()
        {
            RuleFor(x => x.CancellationReason)
                .NotEmpty().WithMessage("İptal nedeni boş olamaz.")
                .MaximumLength(500).WithMessage("İptal nedeni 500 karakterden uzun olamaz.");
        }
    }
}
