using FluentValidation;

namespace FixNow.Application.Features.CustomerProfiles.Commands.AddCustomerPaymentMethod;

public sealed class AddCustomerPaymentMethodCommandValidator
    : AbstractValidator<AddCustomerPaymentMethodCommand>
{
    public AddCustomerPaymentMethodCommandValidator()
    {
        RuleFor(x => x.Type)
            .IsInEnum()
            .WithErrorCode("CustomerPaymentMethod.TypeRequired");
    }
}
