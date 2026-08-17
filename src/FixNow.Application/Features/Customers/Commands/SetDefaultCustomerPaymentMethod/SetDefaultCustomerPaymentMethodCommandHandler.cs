using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;

namespace FixNow.Application.Features.CustomerProfiles.Commands.SetDefaultCustomerPaymentMethod;

public sealed class SetDefaultCustomerPaymentMethodCommandHandler(
    ICustomerRepository customerRepository,
    ICurrentUser currentUser)
    : ICommandHandler<SetDefaultCustomerPaymentMethodCommand, Result<Success>>
{
    public async Task<Result<Success>> Handle(
        SetDefaultCustomerPaymentMethodCommand command,
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

        // 2. Set the default payment method on the profile.
        var setDefaultResult = customerProfile.SetDefaultPaymentMethod(
            command.PaymentMethodId);

        if (setDefaultResult.IsError)
        {
            return setDefaultResult.Errors;
        }

        // 3. Persist changes.
        customerRepository.Update(customerProfile);

        return Result.Success;
    }
}
