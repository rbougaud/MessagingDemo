using Contracts.Movie;
using MediatR;

namespace Application.Events.Movies.Movie_Deleted;

public readonly record struct MovieDeletedEvent(MovieDeleted MovieDeleted) : INotification;
