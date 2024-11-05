using Contracts.Movie;
using MediatR;

namespace Application.Events.Movies.Movie_Created;

public readonly record struct MovieCreatedEvent(MovieCreated MovieCreated) : INotification;
