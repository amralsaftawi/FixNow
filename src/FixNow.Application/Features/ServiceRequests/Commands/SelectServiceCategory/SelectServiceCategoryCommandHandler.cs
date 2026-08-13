using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;

namespace FixNow.Application.Features.ServiceRequests.Commands.SelectServiceCategory;

public sealed class SelectServiceCategoryCommandHandler(
    ICustomerRepository customerRepository,
    IServiceRequestRepository serviceRequestRepository,
    IServiceCategoryRepository serviceCategoryRepository,
    ICurrentUser currentUser)
    : ICommandHandler<SelectServiceCategoryCommand, Result<Success>>
{
    public async Task<Result<Success>> Handle(
        SelectServiceCategoryCommand command,
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

        // 4. Validate the selected service category exists and is active.
        var serviceCategories = await serviceCategoryRepository.GetByIdsAsync(
            [command.ServiceCategoryId],
            cancellationToken);

        if (serviceCategories.Count != 1
            || serviceCategories.Any(category => !category.IsActive))
        {
            return ServiceCategoryErrors.NotFound;
        }

        // 5. Apply the category through the domain model.
        var changeResult = serviceRequest.ChangeServiceCategory(
            command.ServiceCategoryId);

        if (changeResult.IsError)
        {
            return changeResult.Errors;
        }

        // 6. Persist the change (committed by the transaction pipeline).
        serviceRequestRepository.Update(serviceRequest);

        return Result.Success;
    }
}
