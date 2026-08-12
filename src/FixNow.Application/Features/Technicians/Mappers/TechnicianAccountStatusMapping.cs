using FixNow.Application.Features.TechnicianProfiles.Dtos.Responses;

namespace FixNow.Application.Features.TechnicianProfiles.Mappers;

public static class TechnicianAccountStatusMapping
{
    public static TechnicianAccountStatusResponse ToTechnicianAccountStatusResponse(
        this TechnicianProfile technicianProfile,
        User user)
    {
        ArgumentNullException.ThrowIfNull(technicianProfile);

        ArgumentNullException.ThrowIfNull(user);

        return new TechnicianAccountStatusResponse(
            TechnicianProfileId: technicianProfile.Id,
            UserId: user.Id,
            Status: user.AccountStatus);
    }
}
