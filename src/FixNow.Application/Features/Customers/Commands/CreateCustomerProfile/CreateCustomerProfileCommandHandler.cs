using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;

namespace FixNow.Application.Features.CustomerProfiles.Commands.CreateCustomerProfile;

public sealed class CreateCustomerProfileCommandHandler(
    ICustomerRepository customerRepository,
    ICurrentUser currentUser)
    : ICommandHandler<CreateCustomerProfileCommand, Result<Created>>
{
    public async Task<Result<Created>> Handle(
        CreateCustomerProfileCommand command,
        CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId;

        var profileExists = await customerRepository.ExistsByUserIdAsync(
            userId,
            cancellationToken);

        if (profileExists)
        {
            return CustomerProfileErrors.AlreadyExists;
        }

        var createResult = CustomerProfile.Create(
            id: Guid.NewGuid(),
            userId: userId);

        if (createResult.IsError)
        {
            return createResult.Errors;
        }

        await customerRepository.AddAsync(
            createResult.Value,
            cancellationToken);

        return Result.Created;
    }
}
