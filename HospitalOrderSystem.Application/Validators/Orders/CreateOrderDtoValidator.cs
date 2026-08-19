using FluentValidation;
using HospitalOrderSystem.Application.DTOs.Orders;

namespace HospitalOrderSystem.Application.Validators.Orders
{
    public class CreateOrderDtoValidator : AbstractValidator<CreateOrderDto>
    {
        public CreateOrderDtoValidator()
        {
            RuleFor(order => order)
                .Must(order => !string.IsNullOrWhiteSpace(order.PatientTcNo) ||
                               (!string.IsNullOrWhiteSpace(order.PatientFirstName) && !string.IsNullOrWhiteSpace(order.PatientLastName)))
                .WithMessage("Lütfen hastanın TC Kimlik Numarasını ya da Ad ve Soyadını giriniz.");


            RuleFor(order => order.OrderType)
                .IsInEnum()
                .WithMessage("Geçerli bir order türü seçilmelidir.");

            RuleFor(order => order.Title)
                .NotEmpty()
                .WithMessage("Başlık zorunludur.");
        }
    }
}
