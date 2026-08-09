using ApplicationForgotPasswordResponse = FixNow.Application.Features.Identity.Commands.ForgotPassword.ForgotPasswordResponse;
using ContractForgotPasswordResponse = FixNow.Contracts.Responses.ForgotPasswordResponse;

namespace FixNow.Api.Mappings.Identity;

public static class ForgotPasswordMapping
{
    public static ContractForgotPasswordResponse ToContractResponse(
        this ApplicationForgotPasswordResponse response)
        => new(response.Message);
}
