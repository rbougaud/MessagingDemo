using Application.Events.Movies.Movie_Updated;
using Application.Services.Mailing;
using Contracts.Movie;
using Domain.Abstraction.Customers;
using MassTransit;
using MediatR;
using Serilog;

namespace Application.Consumers.Movies;

public class MovieUpdatedConsumer(ILogger logger, IMediator mediator, ICustomerRepositoryReader repositoryReader) : IConsumer<MovieUpdated>
{
    private readonly ILogger _logger = logger;
    private readonly IMediator _mediator = mediator;
    private readonly ICustomerRepositoryReader _repositoryReader = repositoryReader;

    public async Task Consume(ConsumeContext<MovieUpdated> context)
    {
        _logger.Information(nameof(MovieUpdatedConsumer));
        await _mediator.Publish(new MovieUpdatedEvent(context.Message));

        var emails = await _repositoryReader.GetAllMailsAsync();
        foreach (var e in emails)
        {
            var emailService = new EmailService(_logger, "smtp.example.com", 587, "votre_email@example.com", "votre_mot_de_passe");
            emailService.SendEmail(e, "Update on movie", $"An update has been done on the movie {context.Message.Title}. Customer price {context.Message.Saleprice}");
        }
    }
}
