using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;

namespace FixNow.Application.Features.ServiceRequests.Commands.SelectProblemType;

public sealed class SelectProblemTypeCommandHandler(
    ICustomerRepository customerRepository,
    IServiceRequestRepository serviceRequestRepository,
    IProblemTypeRepository problemTypeRepository,
    ICurrentUser currentUser)
    : ICommandHandler<SelectProblemTypeCommand, Result<Success>>
{
    public async Task<Result<Success>> Handle(
        SelectProblemTypeCommand command,
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

        // 4. Load the requested problem type.
        var problemType = await problemTypeRepository.GetByIdAsync(
            command.ProblemTypeId,
            cancellationToken);

        if (problemType is null || !problemType.IsActive)
        {
            return ProblemTypeErrors.NotFound;
        }

        // 5. Verify the problem type is valid for the service request's
        //    selected service category.
        if (problemType.ServiceCategoryId != serviceRequest.ServiceCategoryId)
        {
            return ServiceRequestErrors.ProblemTypeIncompatible;
        }

        // 6. Apply the problem type through the domain model.
        var changeResult = serviceRequest.ChangeProblemType(
            command.ProblemTypeId);

        if (changeResult.IsError)
        {
            return changeResult.Errors;
        }

        // 7. Persist the change (committed by the transaction pipeline).
        serviceRequestRepository.Update(serviceRequest);

        return Result.Success;
    }
}
