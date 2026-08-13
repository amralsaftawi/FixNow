using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;

namespace FixNow.Application.Features.ServiceRequests.Commands.SetEstimatedCost;

public sealed class SetEstimatedCostCommandHandler(
    ICustomerRepository customerRepository,
    IServiceRequestRepository serviceRequestRepository,
    ICurrentUser currentUser)
    : ICommandHandler<SetEstimatedCostCommand, Result<Success>>
{
    public async Task<Result<Success>> Handle(
        SetEstimatedCostCommand command,
        CancellationToken cancellationToken)
    {
        // 1. Find the current user's customer profile (ownership is derived
        //    from the authenticated user, never from the client).
        var customerProfile = await customerRepository.GetByUserIdAsync(
            currentUser.UserId,
            cancellationToken);

        if (customerProfile is null)
        {
            return CustomerProfileErrors.NotFound;
        }

        // 2. Load the service request.
        var serviceRequest = await serviceRequestRepository.GetByIdAsync(
            command.ServiceRequestId,
            cancellationToken);

        if (serviceRequest is null)
        {
            return ServiceRequestErrors.NotFound;
        }

        // 3. Verify the service request belongs to the current customer.
        if (serviceRequest.CustomerProfileId != customerProfile.Id)
        {
            return ServiceRequestErrors.NotFound;
        }

        // 4. Build the monetary value (domain validates the amount).
        var estimatedCostResult = Money.Create(
            command.Amount,
            command.Currency);

        if (estimatedCostResult.IsError)
        {
            return estimatedCostResult.Errors;
        }

        // 5. Apply the estimated cost through the domain model.
        var setResult = serviceRequest.SetEstimatedCost(
            estimatedCostResult.Value);

        if (setResult.IsError)
        {
            return setResult.Errors;
        }

        // 6. Persist the change (committed by the transaction pipeline).
        serviceRequestRepository.Update(serviceRequest);

        return Result.Success;
    }
}
