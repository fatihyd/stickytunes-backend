using FluentValidation;
using StickyTunes.Business.DTOs;

namespace StickyTunes.Business.Validators;

public class CreateCommentRequestValidator : AbstractValidator<CreateCommentRequest>
{
    public CreateCommentRequestValidator()
    {
        RuleFor(comment => comment.SpotifyUrl)
            .NotEmpty().WithMessage("The Spotify Url is required.");
        
        RuleFor(comment => comment.Text)
            .NotEmpty().WithMessage("The Text is required.")
            .MaximumLength(200).WithMessage("The Text must not exceed 200 characters.");
    }
}