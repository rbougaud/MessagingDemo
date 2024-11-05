using Application.Services.Customers.Commands.AddCustomer;
using Application.Services.Customers.Commands.DeleteCustomerByFullName;
using Application.Services.Customers.Commands.UpdateCustomer;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace OutboxHf.Controllers.Customers;

[ApiController]
[Route("api/[controller]")]
public class CustomerWriterController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost("[action]")]
    public async Task<IActionResult> Add([FromBody] AddCustomerCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsFailure ? BadRequest(result.Error) : Ok(result.Value);
    }

    [HttpPut("[action]")]
    public async Task<IActionResult> Update([FromBody] UpdateCustomerCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsFailure ? BadRequest(result.Error) : Ok(result.Value);
    }

    [HttpDelete("[action]")]
    public async Task<IActionResult> DeleteCustomerByFullName([FromBody] DeleteCustomerByFullNameCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsFailure ? BadRequest(result.Error) : Ok(result.Value);
    }
}
