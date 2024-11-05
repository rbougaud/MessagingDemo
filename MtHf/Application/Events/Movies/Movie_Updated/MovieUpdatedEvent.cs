using Contracts.Movie;
using MediatR;

namespace Application.Events.Movies.Movie_Updated;

public readonly record struct MovieUpdatedEvent(MovieUpdated MovieUpdated) : INotification;
