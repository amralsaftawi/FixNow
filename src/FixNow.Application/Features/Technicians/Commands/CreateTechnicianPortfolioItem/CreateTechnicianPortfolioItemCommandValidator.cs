using FluentValidation;

namespace FixNow.Application.Features.TechnicianProfiles.Commands.CreateTechnicianPortfolioItem;

public sealed class CreateTechnicianPortfolioItemCommandValidator
    : AbstractValidator<CreateTechnicianPortfolioItemCommand>
{
    public CreateTechnicianPortfolioItemCommandValidator()
    {
        ValidateTitle();

        ValidateDescription();

        ValidateMediaKeys();
    }

    private void ValidateTitle()
    {
        RuleFor(x => x.Title)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithErrorCode("TechnicianPortfolioItem.TitleRequired")
            .MaximumLength(150)
            .WithErrorCode("TechnicianPortfolioItem.TitleTooLong");
    }

    private void ValidateDescription()
    {
        RuleFor(x => x.Description)
            .MaximumLength(1000)
            .WithErrorCode("TechnicianPortfolioItem.DescriptionTooLong");
    }

    private void ValidateMediaKeys()
    {
        RuleForEach(x => x.MediaKeys)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithErrorCode("TechnicianPortfolioItem.MediaKeyRequired")
            .MaximumLength(500)
            .WithErrorCode("TechnicianPortfolioItem.MediaKeyTooLong");
    }
}
