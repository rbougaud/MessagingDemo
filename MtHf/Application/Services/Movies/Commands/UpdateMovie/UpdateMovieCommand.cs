using Domain.Common.Helper;
using MediatR;

namespace Application.Services.Movies.Commands.UpdateMovie;

public record UpdateMovieCommand(Guid Id, string Title, string Author, DateOnly ReleaseDate, double PurchasePrice, double SalePrice) : IRequest<Result<UpdateMovieResponse, List<string>>>;
