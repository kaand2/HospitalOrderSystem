using FluentValidation;
using HospitalOrderSystem.Application.DTOs.Orders;

namespace HospitalOrderSystem.Application.Validators.Orders
{
    public class UpdateOrderDtoValidator : AbstractValidator<UpdateOrderDto>
    {
        public UpdateOrderDtoValidator()
        {
            RuleFor(order => order.OrderType)
                .IsInEnum()
                .WithMessage("Geçerli bir order türü seçilmelidir.");

            RuleFor(order => order.Status)
                .IsInEnum()
                .WithMessage("Geçerli bir durum seçilmelidir.");

            RuleFor(order => order.Title)
                .NotEmpty()
                .WithMessage("Başlık zorunludur.");
        }
    }
}
