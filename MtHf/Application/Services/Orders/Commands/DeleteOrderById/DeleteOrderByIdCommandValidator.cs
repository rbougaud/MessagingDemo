using FluentValidation;

namespace Application.Services.Orders.Commands.DeleteOrderById;

public class DeleteOrderByIdCommandValidator : AbstractValidator<DeleteOrderByIdCommand>
{
    public DeleteOrderByIdCommandValidator()
    {
        RuleFor(x => x.OrderId).NotEmpty().WithMessage("Id is required.");
    }
}
