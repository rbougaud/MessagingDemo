using Domain.Abstraction.Movies;

namespace Application.Services.Movies.Queries.GetMovieById;

public record GetMovieByIdResponse(IMovieDto MovieDto);
