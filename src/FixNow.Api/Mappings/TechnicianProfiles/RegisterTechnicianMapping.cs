using ApplicationRegisterTechnicianResponse =
    FixNow.Application.Features.TechnicianProfiles.Commands.RegisterTechnician.RegisterTechnicianResponse;
using ContractRegisterTechnicianResponse = FixNow.Contracts.Responses.RegisterTechnicianResponse;

namespace FixNow.Api.Mappings.TechnicianProfiles;

public static class RegisterTechnicianMapping
{
    public static ContractRegisterTechnicianResponse ToContractResponse(
        this ApplicationRegisterTechnicianResponse response)
        => new(
            TechnicianProfileId: response.TechnicianProfileId,
            UserId: response.UserId,
            VerificationStatus: response.VerificationStatus,
            Availability: response.Availability,
            YearsOfExperience: response.YearsOfExperience,
            Bio: response.Bio,
            NationalIdImageKey: response.NationalIdImageKey,
            IsProfileCompleted: response.IsProfileCompleted);
}
