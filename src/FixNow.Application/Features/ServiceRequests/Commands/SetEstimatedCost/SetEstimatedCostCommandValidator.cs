using FluentValidation;

namespace FixNow.Application.Features.ServiceRequests.Commands.SetEstimatedCost;

public sealed class SetEstimatedCostCommandValidator
    : AbstractValidator<SetEstimatedCostCommand>
{
    public SetEstimatedCostCommandValidator()
    {
        RuleFor(x => x.ServiceRequestId)
            .NotEmpty()
            .WithErrorCode("ServiceRequest.IdRequired");

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithErrorCode("Money.Amount.Invalid");

        RuleFor(x => x.Currency)
            .IsInEnum()
            .WithErrorCode("Money.Currency.Invalid");
    }
}
