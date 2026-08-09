using ApplicationResetPasswordResponse = FixNow.Application.Features.Identity.Commands.ResetPassword.ResetPasswordResponse;
using ContractResetPasswordResponse = FixNow.Contracts.Responses.ResetPasswordResponse;

namespace FixNow.Api.Mappings.Identity;

public static class ResetPasswordMapping
{
    public static ContractResetPasswordResponse ToContractResponse(
        this ApplicationResetPasswordResponse response)
        => new(response.Message);
}
