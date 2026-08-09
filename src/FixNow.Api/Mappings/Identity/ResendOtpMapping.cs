using ApplicationResendOtpResponse = FixNow.Application.Features.Identity.Commands.ResendOtp.ResendOtpResponse;
using ContractResendOtpResponse = FixNow.Contracts.Responses.ResendOtpResponse;

namespace FixNow.Api.Mappings.Identity;

public static class ResendOtpMapping
{
    public static ContractResendOtpResponse ToContractResponse(
        this ApplicationResendOtpResponse response)
        => new(response.Message);
}
