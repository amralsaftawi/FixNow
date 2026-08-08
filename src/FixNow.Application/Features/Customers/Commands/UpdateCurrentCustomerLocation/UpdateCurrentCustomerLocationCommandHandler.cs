using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;

namespace FixNow.Application.Features.CustomerProfiles.Commands.UpdateCurrentCustomerLocation;

public sealed class UpdateCurrentCustomerLocationCommandHandler(
    ICustomerRepository customerRepository,
    ICurrentUser currentUser)
    : ICommandHandler<UpdateCurrentCustomerLocationCommand, Result<Success>>
{
    public async Task<Result<Success>> Handle(
        UpdateCurrentCustomerLocationCommand command,
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

        // 2. Update the current location (single record, updated in place).
        var updateResult = customerProfile.UpdateCurrentLocation(
            command.Latitude,
            command.Longitude);

        if (updateResult.IsError)
        {
            return updateResult.Errors;
        }

        // 3. Persist changes.
        customerRepository.Update(customerProfile);

        return Result.Success;
    }
}
