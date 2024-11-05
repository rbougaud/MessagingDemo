using Application.Services.Movies.Queries.GetMovieById;
using Application.Services.Movies.Queries.GetMovies;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace OutboxHf.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MovieReaderController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("[action]")]
    public async Task<IActionResult> GetMovieById(GetMovieByIdRequest request)
    {
        var query = new GetMovieByIdQuery(request.MovieId);
        var result = await _mediator.Send(query);
        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> GetMovies()
    {
        var result = await _mediator.Send(new GetMoviesQuery());
        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
    }
}
