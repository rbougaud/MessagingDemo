using Domain.Common.Helper;
using MediatR;

namespace Application.Services.Movies.Queries.GetMovies;

public record GetMoviesQuery() : IRequest<Result<GetMoviesResponse, string>>;
