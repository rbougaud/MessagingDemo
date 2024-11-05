using Domain.Common.Helper;
using MediatR;

namespace Application.Services.Movies.Commands.AddMovie;

public record AddMovieCommand(string Title, string Author, DateOnly ReleaseDate, double PurchasePrice) : IRequest<Result<AddMovieResponse, List<string>>>;
