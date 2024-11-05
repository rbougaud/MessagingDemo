using Domain.Common.Helper;
using MediatR;

namespace Application.Services.Movies.Commands.DeleteMovieByTitle;

public record DeleteMovieByTitleCommand(Guid Id, string Title, string Author) : IRequest<Result<DeleteMovieByTitleCommandResponse, List<string>>>;
