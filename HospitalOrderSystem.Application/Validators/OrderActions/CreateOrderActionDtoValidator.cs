using FluentValidation;
using HospitalOrderSystem.Application.DTOs.OrderActions;

namespace HospitalOrderSystem.Application.Validators.OrderActions
{
    public class CreateOrderActionDtoValidator : AbstractValidator<CreateOrderActionDto>
    {
        public CreateOrderActionDtoValidator()
        {
            RuleFor(action => action.OrderId)
                .GreaterThan(0)
                .WithMessage("Geçerli bir sipariş seçilmelidir.");

            RuleFor(action => action.UserId)
                .GreaterThan(0)
                .WithMessage("Geçerli bir kullanıcı olmalıdır.");

            RuleFor(action => action.ActionType)
                .IsInEnum()
                .WithMessage("Geçerli bir işlem türü seçilmelidir.");
        }
    }
}
