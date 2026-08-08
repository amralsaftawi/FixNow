using FluentValidation;

namespace FixNow.Application.Features.CustomerProfiles.Commands.RemoveCustomerAddress;

public sealed class RemoveCustomerAddressCommandValidator
    : AbstractValidator<RemoveCustomerAddressCommand>
{
    public RemoveCustomerAddressCommandValidator()
    {
        RuleFor(x => x.AddressId)
            .NotEmpty()
            .WithErrorCode("Address.IdRequired");
    }
}
