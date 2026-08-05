using ApplicationSendOtpResponse = FixNow.Application.Features.Identity.Commands.SendOtp.SendOtpResponse;
using ContractSendOtpResponse = FixNow.Contracts.Responses.SendOtpResponse;

namespace FixNow.Api.Mappings.Identity;

public static class SendOtpMapping
{
    public static ContractSendOtpResponse ToContractResponse(
        this ApplicationSendOtpResponse response)
        => new(response.Message);
}
