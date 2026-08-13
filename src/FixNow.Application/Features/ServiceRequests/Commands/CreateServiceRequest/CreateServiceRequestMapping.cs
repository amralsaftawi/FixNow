namespace FixNow.Application.Features.ServiceRequests.Commands.CreateServiceRequest;

public static class CreateServiceRequestMapping
{
    public static CreateServiceRequestResponse ToCreateServiceRequestResponse(
        this ServiceRequest entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new CreateServiceRequestResponse(
            Id: entity.Id,
            CustomerProfileId: entity.CustomerProfileId,
            AddressId: entity.AddressId,
            ServiceCategoryId: entity.ServiceCategoryId,
            Description: entity.Description,
            Priority: entity.Priority,
            Status: entity.Status,
            RequestedAt: entity.RequestedAt,
            ScheduledAt: entity.ScheduledAt);
    }
}
