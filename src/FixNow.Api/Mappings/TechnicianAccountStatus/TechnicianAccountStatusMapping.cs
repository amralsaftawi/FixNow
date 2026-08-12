using ApplicationTechnicianAccountStatusResponse =
    FixNow.Application.Features.TechnicianProfiles.Dtos.Responses.TechnicianAccountStatusResponse;
using ContractTechnicianAccountStatusResponse =
    FixNow.Contracts.Responses.TechnicianAccountStatusResponse;

namespace FixNow.Api.Mappings.TechnicianAccountStatus;

public static class TechnicianAccountStatusMapping
{
    public static ContractTechnicianAccountStatusResponse ToContractResponse(
        this ApplicationTechnicianAccountStatusResponse response)
        => new(
            TechnicianProfileId: response.TechnicianProfileId,
            UserId: response.UserId,
            Status: response.Status);
}
