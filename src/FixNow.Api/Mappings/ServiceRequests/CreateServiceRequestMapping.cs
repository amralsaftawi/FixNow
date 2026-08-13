using ApplicationCreateServiceRequestResponse =
    FixNow.Application.Features.ServiceRequests.Commands.CreateServiceRequest.CreateServiceRequestResponse;
using ContractCreateServiceRequestResponse = FixNow.Contracts.Responses.CreateServiceRequestResponse;

namespace FixNow.Api.Mappings.ServiceRequests;

public static class CreateServiceRequestMapping
{
    public static ContractCreateServiceRequestResponse ToContractResponse(
        this ApplicationCreateServiceRequestResponse response)
        => new(
            Id: response.Id,
            CustomerProfileId: response.CustomerProfileId,
            AddressId: response.AddressId,
            ServiceCategoryId: response.ServiceCategoryId,
            Description: response.Description,
            Priority: response.Priority,
            Status: response.Status,
            RequestedAt: response.RequestedAt,
            ScheduledAt: response.ScheduledAt);
}
