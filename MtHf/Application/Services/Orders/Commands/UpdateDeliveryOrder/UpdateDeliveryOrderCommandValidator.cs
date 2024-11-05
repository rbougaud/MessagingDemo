using FluentValidation;

namespace Application.Services.Orders.Commands.UpdateDeliveryOrder;

public class UpdateDeliveryOrderCommandValidator : AbstractValidator<UpdateDeliveryOrderCommand>
{
    public UpdateDeliveryOrderCommandValidator()
    {
        RuleFor(x => x.OrderId).NotEmpty().WithMessage("Id is required.");
        RuleFor(x => x.DeliveryDate).GreaterThan(DateOnly.FromDateTime(DateTime.Now)).WithMessage("Delivery date must be in the future.");
    }
}
