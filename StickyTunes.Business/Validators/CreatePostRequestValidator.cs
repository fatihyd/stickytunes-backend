using FluentValidation;
using StickyTunes.Business.DTOs;

namespace StickyTunes.Business.Validators;

public class CreatePostRequestValidator : AbstractValidator<CreatePostRequest>
{
    public CreatePostRequestValidator()
    {
        RuleFor(post => post.SpotifyUrl)
            .NotEmpty().WithMessage("The Spotify Url is required.");
        
        RuleFor(post => post.Text)
            .NotEmpty().WithMessage("The Text is required.")
            .MaximumLength(200).WithMessage("The Text must not exceed 200 characters.");
    }
}