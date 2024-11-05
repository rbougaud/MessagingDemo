using Application.Common.Dto.Movies;

namespace Application.Services.Movies.Queries.GetMovies;

public record GetMoviesResponse(IReadOnlyList<MovieDto> Movies);
