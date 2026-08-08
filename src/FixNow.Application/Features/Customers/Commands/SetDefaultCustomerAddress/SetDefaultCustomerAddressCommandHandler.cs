using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;

namespace FixNow.Application.Features.CustomerProfiles.Commands.SetDefaultCustomerAddress;

public sealed class SetDefaultCustomerAddressCommandHandler(
    ICustomerRepository customerRepository,
    ICurrentUser currentUser)
    : ICommandHandler<SetDefaultCustomerAddressCommand, Result<Success>>
{
    public async Task<Result<Success>> Handle(
        SetDefaultCustomerAddressCommand command,
        CancellationToken cancellationToken)
    {
        // 1. Find the current user's customer profile.
        var customerProfile = await customerRepository.GetByUserIdAsync(
            currentUser.UserId,
            cancellationToken);

        if (customerProfile is null)
        {
            return CustomerProfileErrors.NotFound;
        }

        // 2. Set the default address on the profile.
        var setDefaultResult = customerProfile.SetDefaultAddress(
            command.AddressId);

        if (setDefaultResult.IsError)
        {
            return setDefaultResult.Errors;
        }

        // 3. Persist changes.
        customerRepository.Update(customerProfile);

        return Result.Success;
    }
}
