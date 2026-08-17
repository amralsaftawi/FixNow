using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;

namespace FixNow.Application.Features.CustomerProfiles.Commands.AddCustomerPaymentMethod;

public sealed class AddCustomerPaymentMethodCommandHandler(
    ICustomerRepository customerRepository,
    ICurrentUser currentUser)
    : ICommandHandler<AddCustomerPaymentMethodCommand, Result<Created>>
{
    public async Task<Result<Created>> Handle(
        AddCustomerPaymentMethodCommand command,
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

        // 2. Create the payment method.
        var paymentMethodResult = CustomerPaymentMethod.Create(
            id: Guid.NewGuid(),
            customerProfileId: customerProfile.Id,
            type: command.Type);

        if (paymentMethodResult.IsError)
        {
            return paymentMethodResult.Errors;
        }

        // 3. Add the payment method to the profile.
        var addResult = customerProfile.AddPaymentMethod(paymentMethodResult.Value);

        if (addResult.IsError)
        {
            return addResult.Errors;
        }

        // 4. Persist changes.
        customerRepository.Update(customerProfile);

        return Result.Created;
    }
}
