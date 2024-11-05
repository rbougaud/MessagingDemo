using Application.Services.Orders.Commands.AddOrder;
using Application.Services.Orders.Commands.DeleteOrderById;
using Application.Services.Orders.Commands.UpdateDeliveryOrder;
using Application.Services.Orders.Commands.UpdateOrderValidatePayment;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace OutboxHf.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderWriterController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost("[action]")]
    public async Task<IActionResult> AddOrder([FromBody] AddOrderCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsFailure ? BadRequest(result.Error) : Ok(result.Value);
    }

    [HttpPut("[action]")]
    public async Task<IActionResult> UpdateDeliveryOrder([FromBody] UpdateDeliveryOrderCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsFailure ? BadRequest(result.Error) : Ok(result.Value);
    }

    [HttpPut("[action]")]
    public async Task<IActionResult> UpdateOrderValidatePayment([FromBody] UpdateOrderValidatePaymentCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsFailure ? BadRequest(result.Error) : Ok(result.Value);
    }

    [HttpDelete("[action]")]
    public async Task<IActionResult> DeleteOrderById([FromBody] DeleteOrderByIdCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsFailure ? BadRequest(result.Error) : Ok(result.Value);
    }
}
