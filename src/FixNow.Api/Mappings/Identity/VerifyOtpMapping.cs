using ApplicationVerifyOtpResponse = FixNow.Application.Features.Identity.Commands.VerifyOtp.VerifyOtpResponse;
using ContractVerifyOtpResponse = FixNow.Contracts.Responses.VerifyOtpResponse;

namespace FixNow.Api.Mappings.Identity;

public static class VerifyOtpMapping
{
    public static ContractVerifyOtpResponse ToContractResponse(
        this ApplicationVerifyOtpResponse response)
        => new(response.Message);
}
