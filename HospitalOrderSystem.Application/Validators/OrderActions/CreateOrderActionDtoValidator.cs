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
                .WithMessage("Geçerli bir order seçilmelidir.");

            RuleFor(action => action.ActionType)
                .IsInEnum()
                .WithMessage("Geçerli bir işlem türü seçilmelidir.");
        }
    }
}
