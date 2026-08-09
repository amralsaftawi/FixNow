using ApplicationGetCurrentUserResponse = FixNow.Application.Features.Identity.Queries.GetCurrentUser.GetCurrentUserResponse;
using ContractGetCurrentUserResponse = FixNow.Contracts.Responses.GetCurrentUserResponse;

namespace FixNow.Api.Mappings.Identity;

public static class GetCurrentUserMapping
{
    public static ContractGetCurrentUserResponse ToContractResponse(
        this ApplicationGetCurrentUserResponse response)
        => new(
            Id: response.Id,
            FirstName: response.FirstName,
            LastName: response.LastName,
            Email: response.Email,
            PhoneNumber: response.PhoneNumber);
}
