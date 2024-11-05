using Application.Services.Orders.Queries.GetOrderById;
using Application.Services.Orders.Queries.GetOrders;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace OutboxHf.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderReaderController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("[action]")]
    public async Task<IActionResult> GetOrderById(GetOrderByIdRequest request)
    {
        var query = new GetOrderByIdQuery(request.OrderId);
        var result = await _mediator.Send(query);
        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> GetOrders()
    {
        var result = await _mediator.Send(new GetOrdersQuery());
        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
    }
}
