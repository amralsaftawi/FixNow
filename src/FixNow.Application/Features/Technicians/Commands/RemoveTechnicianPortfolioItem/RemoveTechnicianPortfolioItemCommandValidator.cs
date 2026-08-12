using FluentValidation;

namespace FixNow.Application.Features.TechnicianProfiles.Commands.RemoveTechnicianPortfolioItem;

public sealed class RemoveTechnicianPortfolioItemCommandValidator
    : AbstractValidator<RemoveTechnicianPortfolioItemCommand>
{
    public RemoveTechnicianPortfolioItemCommandValidator()
    {
        RuleFor(x => x.PortfolioItemId)
            .NotEmpty()
            .WithErrorCode("TechnicianPortfolioItem.IdRequired");
    }
}
