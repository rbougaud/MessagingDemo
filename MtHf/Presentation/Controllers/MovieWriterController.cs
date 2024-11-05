using Application.Services.Movies.Commands.AddMovie;
using Application.Services.Movies.Commands.DeleteMovieByTitle;
using Application.Services.Movies.Commands.UpdateMovie;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace OutboxHf.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MovieWriterController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost("[action]")]
    public async Task<IActionResult> AddMovie([FromBody] AddMovieCommand command)
    {
        //var test = command with { Author = string.Empty }; //Ajouter pour demo erreur
        var result = await _mediator.Send(command);
        return result.IsFailure ? BadRequest(result.Error) : Ok(result.Value);
    }

    [HttpPut("[action]")]
    public async Task<IActionResult> UpdateMovie([FromBody] UpdateMovieCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsFailure ? BadRequest(result.Error) : Ok(result.Value);
    }

    [HttpDelete("[action]")]
    public async Task<IActionResult> DeleteMovieByTitle([FromBody] DeleteMovieByTitleCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsFailure ? BadRequest(result.Error) : Ok(result.Value);
    }
}
