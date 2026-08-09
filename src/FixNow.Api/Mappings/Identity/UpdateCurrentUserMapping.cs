using ApplicationUpdateCurrentUserResponse = FixNow.Application.Features.Identity.Commands.UpdateCurrentUser.UpdateCurrentUserResponse;
using ContractUpdateCurrentUserResponse = FixNow.Contracts.Responses.UpdateCurrentUserResponse;

namespace FixNow.Api.Mappings.Identity;

public static class UpdateCurrentUserMapping
{
    public static ContractUpdateCurrentUserResponse ToContractResponse(
        this ApplicationUpdateCurrentUserResponse response)
        => new(
            Id: response.Id,
            FirstName: response.FirstName,
            LastName: response.LastName,
            Email: response.Email,
            PhoneNumber: response.PhoneNumber);
}
