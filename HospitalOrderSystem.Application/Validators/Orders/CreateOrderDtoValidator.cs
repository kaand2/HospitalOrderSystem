using FluentValidation;
using HospitalOrderSystem.Application.DTOs.Orders;

namespace HospitalOrderSystem.Application.Validators.Orders
{
    public class CreateOrderDtoValidator : AbstractValidator<CreateOrderDto>
    {
        public CreateOrderDtoValidator()
        {
            RuleFor(order => order.PatientId)
                .GreaterThan(0)
                .WithMessage("Geçerli bir hasta seçilmelidir.");

            RuleFor(order => order.CreatedByUserId)
                .GreaterThan(0)
                .WithMessage("Geçerli bir kullanıcı olmalıdır.");

            RuleFor(order => order.OrderType)
                .IsInEnum()
                .WithMessage("Geçerli bir order türü seçilmelidir.");

            RuleFor(order => order.Title)
                .NotEmpty()
                .WithMessage("Başlık zorunludur.");
        }
    }
}
