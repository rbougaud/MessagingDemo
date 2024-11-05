using Domain.Common.Helper;
using MediatR;

namespace Application.Services.Movies.Queries.GetMovieById;

public readonly record struct GetMovieByIdQuery(Guid MovieId) : IRequest<Result<GetMovieByIdResponse, string>>;
