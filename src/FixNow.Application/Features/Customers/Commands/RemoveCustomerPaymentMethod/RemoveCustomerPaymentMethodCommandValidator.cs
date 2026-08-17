using FluentValidation;

namespace FixNow.Application.Features.CustomerProfiles.Commands.RemoveCustomerPaymentMethod;

public sealed class RemoveCustomerPaymentMethodCommandValidator
    : AbstractValidator<RemoveCustomerPaymentMethodCommand>
{
    public RemoveCustomerPaymentMethodCommandValidator()
    {
        RuleFor(x => x.PaymentMethodId)
            .NotEmpty()
            .WithErrorCode("CustomerPaymentMethod.IdRequired");
    }
}
