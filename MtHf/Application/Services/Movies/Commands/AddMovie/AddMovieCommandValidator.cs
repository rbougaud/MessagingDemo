using FluentValidation;

namespace Application.Services.Movies.Commands.AddMovie;

public class AddMovieCommandValidator : AbstractValidator<AddMovieCommand>
{
    public AddMovieCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required.");
        RuleFor(x => x.Author).NotEmpty().WithMessage("Author is required.");
        RuleFor(x => x.ReleaseDate).LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now)).WithMessage("Release date must be in the past.");
        RuleFor(x => x.PurchasePrice).GreaterThan(0).WithMessage("The Purchase Price must be more than 0");
    }
}
