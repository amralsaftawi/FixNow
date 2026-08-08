using FluentValidation;

namespace FixNow.Application.Features.CustomerProfiles.Commands.SetDefaultCustomerAddress;

public sealed class SetDefaultCustomerAddressCommandValidator
    : AbstractValidator<SetDefaultCustomerAddressCommand>
{
    public SetDefaultCustomerAddressCommandValidator()
    {
        RuleFor(x => x.AddressId)
            .NotEmpty()
            .WithErrorCode("Address.IdRequired");
    }
}
