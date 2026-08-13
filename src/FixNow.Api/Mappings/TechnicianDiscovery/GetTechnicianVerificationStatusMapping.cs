using ApplicationTechnicianVerificationStatusResponse =
    FixNow.Application.Features.TechnicianProfiles.Dtos.Responses.TechnicianVerificationStatusResponse;
using ContractTechnicianVerificationStatusResponse =
    FixNow.Contracts.Responses.TechnicianVerificationStatusResponse;

namespace FixNow.Api.Mappings.TechnicianDiscovery;

public static class GetTechnicianVerificationStatusMapping
{
    public static ContractTechnicianVerificationStatusResponse ToContractResponse(
        this ApplicationTechnicianVerificationStatusResponse response)
        => new(
            TechnicianProfileId: response.TechnicianProfileId,
            Status: response.Status);
}
