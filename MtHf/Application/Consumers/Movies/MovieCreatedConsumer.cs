using Application.Events.Movies.Movie_Created;
using Application.Services.Mailing;
using Contracts.Movie;
using Domain.Abstraction.Customers;
using MassTransit;
using MediatR;
using Serilog;

namespace Application.Consumers.Movies;

public class MovieCreatedConsumer(ILogger logger, IMediator mediator, ICustomerRepositoryReader repositoryReader) : IConsumer<MovieCreated>
{
    private readonly ILogger _logger = logger;
    private readonly IMediator _mediator = mediator;
    private readonly ICustomerRepositoryReader _repositoryReader = repositoryReader;

    public async Task Consume(ConsumeContext<MovieCreated> context)
    {
        _logger.Information(nameof(MovieCreatedConsumer));
        await _mediator.Publish(new MovieCreatedEvent(context.Message));

        var emails = await _repositoryReader.GetAllMailsAsync();
        foreach (var e in emails)
        {
            var emailService = new EmailService(_logger, "smtp.example.com", 587, "votre_email@example.com", "votre_mot_de_passe");
            emailService.SendEmail(e, "New movie", $"A new movie is available : {context.Message.Title} realized by {context.Message.Author}.");
        }
    }
}
