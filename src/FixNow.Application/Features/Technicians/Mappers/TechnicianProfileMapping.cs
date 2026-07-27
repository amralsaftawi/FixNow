using System.Collections.Generic;
using System.Linq;
using FixNow.Application.Features.TechnicianProfiles.Dtos.Responses;

namespace FixNow.Application.Features.TechnicianProfiles.Mappers;

public static class TechnicianProfileMapping
{
    public static TechnicianProfileResponse ToTechnicianProfileResponse(
        this TechnicianProfile entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new TechnicianProfileResponse(
            TechnicianProfileId: entity.Id,
            UserId: entity.UserId,
            Availability: entity.Availability,
            YearsOfExperience: entity.YearsOfExperience,
            Bio: entity.Bio,
            NationalIdImageKey: entity.NationalIdImageKey,
            IsProfileCompleted: entity.IsProfileCompleted,
            VerificationStatus: entity.VerificationStatus);
    }

    public static List<TechnicianProfileResponse> ToDtos(
        this IEnumerable<TechnicianProfile> entities)
    {
        return entities.Select(ToTechnicianProfileResponse).ToList();
    }
}