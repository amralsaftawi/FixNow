using FluentValidation;

namespace FixNow.Application.Features.TechnicianProfiles.Commands.UpdateTechnicianServicePricing;

public sealed class UpdateTechnicianServicePricingCommandValidator
    : AbstractValidator<UpdateTechnicianServicePricingCommand>
{
    public UpdateTechnicianServicePricingCommandValidator()
    {
        RuleFor(x => x.TechnicianServiceId)
            .NotEmpty()
            .WithErrorCode("TechnicianService.IdRequired");

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithErrorCode("Money.Amount.Invalid");

        RuleFor(x => x.Currency)
            .IsInEnum()
            .WithErrorCode("Money.Currency.Invalid");
    }
}
