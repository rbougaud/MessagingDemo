using Application.Services.CreditCards.AddCreditCard;
using Application.Services.CreditCards.DeleteCreditCardByIdCustomer;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace OutboxHf.Controllers.Customers;

[ApiController]
[Route("[controller]")]
public class CreditCardWriterController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost("[action]")]
    public async Task<IActionResult> AddCreditCard([FromBody] AddCreditCardCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsFailure ? BadRequest(result.Error) : Ok(result.Value);
    }

    [HttpDelete("[action]")]
    public async Task<IActionResult> DeleteCreditCardByIdCustomer([FromBody] DeleteCreditCardByIdCustomerCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsFailure ? BadRequest(result.Error) : Ok(result.Value);
    }
}
