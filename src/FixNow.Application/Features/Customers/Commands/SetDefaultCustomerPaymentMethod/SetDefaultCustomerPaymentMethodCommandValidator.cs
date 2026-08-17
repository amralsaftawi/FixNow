using FluentValidation;

namespace FixNow.Application.Features.CustomerProfiles.Commands.SetDefaultCustomerPaymentMethod;

public sealed class SetDefaultCustomerPaymentMethodCommandValidator
    : AbstractValidator<SetDefaultCustomerPaymentMethodCommand>
{
    public SetDefaultCustomerPaymentMethodCommandValidator()
    {
        RuleFor(x => x.PaymentMethodId)
            .NotEmpty()
            .WithErrorCode("CustomerPaymentMethod.IdRequired");
    }
}
