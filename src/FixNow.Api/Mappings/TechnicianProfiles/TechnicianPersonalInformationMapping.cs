using ApplicationTechnicianPersonalInformationResponse =
    FixNow.Application.Features.TechnicianProfiles.Dtos.Responses.TechnicianPersonalInformationResponse;
using ContractTechnicianPersonalInformationResponse =
    FixNow.Contracts.Responses.TechnicianPersonalInformationResponse;

namespace FixNow.Api.Mappings.TechnicianProfiles;

public static class TechnicianPersonalInformationMapping
{
    public static ContractTechnicianPersonalInformationResponse ToContractResponse(
        this ApplicationTechnicianPersonalInformationResponse response)
        => new(
            UserId: response.UserId,
            TechnicianProfileId: response.TechnicianProfileId,
            FirstName: response.FirstName,
            LastName: response.LastName,
            Email: response.Email,
            PhoneNumber: response.PhoneNumber,
            CountryCode: response.CountryCode,
            PreferredLanguage: response.PreferredLanguage,
            YearsOfExperience: response.YearsOfExperience,
            Bio: response.Bio,
            NationalIdImageKey: response.NationalIdImageKey,
            IsProfileCompleted: response.IsProfileCompleted,
            VerificationStatus: response.VerificationStatus);
}
