using Application.Services.Customers.Queries.GetCustomerById;
using Application.Services.Customers.Queries.GetCustomers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace OutboxHf.Controllers.Customers;

[ApiController]
[Route("api/[controller]")]
public class CustomerReaderController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("[action]")]
    public async Task<IActionResult> GetCustomerById(GetCustomerByIdRequest request)
    {
        var query = new GetCustomerByIdQuery(request.Id);
        var result = await _mediator.Send(query);
        return result.IsFailure ? BadRequest(result.Error) : Ok(result.Value);
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> GetCustomers()
    {
        var result = await _mediator.Send(new GetCustomersQuery());
        return result.IsFailure ? BadRequest(result.Error) : Ok(result.Value);
    }
}
