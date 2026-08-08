using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;

namespace FixNow.Application.Features.CustomerProfiles.Commands.RemoveCustomerAddress;

public sealed class RemoveCustomerAddressCommandHandler(
    ICustomerRepository customerRepository,
    ICurrentUser currentUser)
    : ICommandHandler<RemoveCustomerAddressCommand, Result<Success>>
{
    public async Task<Result<Success>> Handle(
        RemoveCustomerAddressCommand command,
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

        // 2. Remove the address from the profile.
        var removeResult = customerProfile.RemoveAddress(
            command.AddressId);

        if (removeResult.IsError)
        {
            return removeResult.Errors;
        }

        // 3. Persist changes.
        customerRepository.Update(customerProfile);

        return Result.Success;
    }
}
