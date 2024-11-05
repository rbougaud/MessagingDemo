using Application.Services.LoyalAccounts.AddLoyalAccount;
using Application.Services.LoyalAccounts.DeleteLoyaltyAccountByIdCustomer;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace OutboxHf.Controllers.Customers;

[ApiController]
[Route("[controller]")]
public class LoyaltyAccountWriterController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost("[action]")]
    public async Task<IActionResult> AddLoyaltyAccount([FromBody] AddLoyaltyAccountCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsFailure ? BadRequest(result.Error) : Ok(result.Value);
    }

    [HttpDelete("[action]")]
    public async Task<IActionResult> DeleteByFullNameCustomer([FromBody] DeleteLoyaltyAccountByFullNameCustomerCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsFailure ? BadRequest(result.Error) : Ok(result.Value);
    }
}