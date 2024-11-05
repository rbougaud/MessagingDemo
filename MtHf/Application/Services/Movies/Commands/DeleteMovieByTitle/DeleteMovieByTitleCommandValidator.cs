using FluentValidation;

namespace Application.Services.Movies.Commands.DeleteMovieByTitle;

public class DeleteMovieByTitleCommandValidator : AbstractValidator<DeleteMovieByTitleCommand>
{
    public DeleteMovieByTitleCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required.");
        RuleFor(x => x.Author).NotEmpty().WithMessage("Author is required.");
    }
}
