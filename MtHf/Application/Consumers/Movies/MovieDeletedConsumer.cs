using Application.Events.Movies.Movie_Deleted;
using Application.Services.Mailing;
using Contracts.Movie;
using Domain.Abstraction.Customers;
using MassTransit;
using MediatR;
using Serilog;

namespace Application.Consumers.Movies;

public class MovieDeletedConsumer(ILogger logger, IMediator mediator, ICustomerRepositoryReader repositoryReader) : IConsumer<MovieDeleted>
{
    private readonly ILogger _logger = logger;
    private readonly IMediator _mediator = mediator;
    private readonly ICustomerRepositoryReader _repositoryReader = repositoryReader;

    public async Task Consume(ConsumeContext<MovieDeleted> context)
    {
        _logger.Information(nameof(MovieDeletedConsumer));
        await _mediator.Publish(new MovieDeletedEvent(context.Message));

        var emails = await _repositoryReader.GetAllMailsAsync();
        foreach (var e in emails)
        {
            var emailService = new EmailService(_logger, "smtp.example.com", 587, "votre_email@example.com", "votre_mot_de_passe");
            emailService.SendEmail(e, "Not available movie", $"A movie will not be available : {context.Message.Title} realized by {context.Message.Author}.");
        }
    }
}
