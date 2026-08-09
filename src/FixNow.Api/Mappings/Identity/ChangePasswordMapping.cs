using ApplicationChangePasswordResponse = FixNow.Application.Features.Identity.Commands.ChangePassword.ChangePasswordResponse;
using ContractChangePasswordResponse = FixNow.Contracts.Responses.ChangePasswordResponse;

namespace FixNow.Api.Mappings.Identity;

public static class ChangePasswordMapping
{
    public static ContractChangePasswordResponse ToContractResponse(
        this ApplicationChangePasswordResponse response)
        => new(response.Message);
}
