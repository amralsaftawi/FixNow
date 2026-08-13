using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;

namespace FixNow.Application.Features.ServiceRequests.Commands.CreateServiceRequest;

public sealed class CreateServiceRequestCommandHandler(
    ICustomerRepository customerRepository,
    IServiceCategoryRepository serviceCategoryRepository,
    IServiceRequestRepository serviceRequestRepository,
    ICurrentUser currentUser)
    : ICommandHandler<CreateServiceRequestCommand, Result<CreateServiceRequestResponse>>
{
    public async Task<Result<CreateServiceRequestResponse>> Handle(
        CreateServiceRequestCommand command,
        CancellationToken cancellationToken)
    {
        var customerProfile = await customerRepository.GetByUserIdAsync(
            currentUser.UserId,
            cancellationToken);

        if (customerProfile is null)
        {
            return CustomerProfileErrors.NotFound;
        }

        if (customerProfile.Addresses.All(address => address.Id != command.AddressId))
        {
            return CustomerProfileErrors.AddressNotFound;
        }

        var serviceCategories = await serviceCategoryRepository.GetByIdsAsync(
            [command.ServiceCategoryId],
            cancellationToken);

        if (serviceCategories.Count != 1
            || serviceCategories.Any(category => !category.IsActive))
        {
            return ServiceCategoryErrors.NotFound;
        }

        var createResult = ServiceRequest.Create(
            id: Guid.NewGuid(),
            customerProfileId: customerProfile.Id,
            addressId: command.AddressId,
            serviceCategoryId: command.ServiceCategoryId,
            description: command.Description,
            priority: command.Priority,
            scheduledAt: command.ScheduledAt);

        if (createResult.IsError)
        {
            return createResult.Errors;
        }

        await serviceRequestRepository.AddAsync(
            createResult.Value,
            cancellationToken);

        return createResult.Value.ToCreateServiceRequestResponse();
    }
}
