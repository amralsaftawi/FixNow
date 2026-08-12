using System.Collections.Generic;
using System.Linq;
using ApplicationTechnicianExperienceResponse =
    FixNow.Application.Features.TechnicianProfiles.Dtos.Responses.TechnicianExperienceResponse;
using ContractTechnicianExperienceResponse =
    FixNow.Contracts.Responses.TechnicianExperienceResponse;

namespace FixNow.Api.Mappings.TechnicianProfiles;

public static class TechnicianExperienceMapping
{
    public static ContractTechnicianExperienceResponse ToContractResponse(
        this ApplicationTechnicianExperienceResponse response)
        => new(
            TechnicianExperienceId: response.TechnicianExperienceId,
            TechnicianProfileId: response.TechnicianProfileId,
            CompanyName: response.CompanyName,
            Position: response.Position,
            Description: response.Description,
            StartDate: response.StartDate,
            EndDate: response.EndDate,
            IsCurrent: response.IsCurrent);

    public static List<ContractTechnicianExperienceResponse> ToContractResponses(
        this IEnumerable<ApplicationTechnicianExperienceResponse> responses)
        => responses.Select(ToContractResponse).ToList();
}
