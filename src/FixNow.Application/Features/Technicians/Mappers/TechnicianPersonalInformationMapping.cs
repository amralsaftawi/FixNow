using FixNow.Application.Features.TechnicianProfiles.Dtos.Responses;

namespace FixNow.Application.Features.TechnicianProfiles.Mappers;

public static class TechnicianPersonalInformationMapping
{
    public static TechnicianPersonalInformationResponse ToTechnicianPersonalInformationResponse(
        this User user,
        TechnicianProfile technicianProfile)
    {
        ArgumentNullException.ThrowIfNull(user);

        ArgumentNullException.ThrowIfNull(technicianProfile);

        return new TechnicianPersonalInformationResponse(
            UserId: user.Id,
            TechnicianProfileId: technicianProfile.Id,
            FirstName: user.FirstName,
            LastName: user.LastName,
            Email: user.Email?.Value,
            PhoneNumber: user.PhoneNumber.Value,
            CountryCode: user.CountryCode.Value,
            PreferredLanguage: user.PreferredLanguage,
            YearsOfExperience: technicianProfile.YearsOfExperience,
            Bio: technicianProfile.Bio,
            NationalIdImageKey: technicianProfile.NationalIdImageKey,
            IsProfileCompleted: technicianProfile.IsProfileCompleted,
            VerificationStatus: technicianProfile.VerificationStatus);
    }
}
