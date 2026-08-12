using FluentValidation;

namespace FixNow.Application.Features.TechnicianProfiles.Commands.UpdateTechnicianPortfolioItem;

public sealed class UpdateTechnicianPortfolioItemCommandValidator
    : AbstractValidator<UpdateTechnicianPortfolioItemCommand>
{
    public UpdateTechnicianPortfolioItemCommandValidator()
    {
        ValidatePortfolioItemId();

        ValidateTitle();

        ValidateDescription();

        ValidateMediaKeys();
    }

    private void ValidatePortfolioItemId()
    {
        RuleFor(x => x.PortfolioItemId)
            .NotEmpty()
            .WithErrorCode("TechnicianPortfolioItem.IdRequired");
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
