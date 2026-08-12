using System.Collections.Generic;
using System.Linq;
using FixNow.Application.Features.TechnicianProfiles.Dtos.Responses;

namespace FixNow.Application.Features.TechnicianProfiles.Mappers;

public static class TechnicianExperienceMapping
{
    public static TechnicianExperienceResponse ToTechnicianExperienceResponse(
        this TechnicianExperience entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new TechnicianExperienceResponse(
            TechnicianExperienceId: entity.Id,
            TechnicianProfileId: entity.TechnicianProfileId,
            CompanyName: entity.CompanyName,
            Position: entity.Position,
            Description: entity.Description,
            StartDate: entity.StartDate,
            EndDate: entity.EndDate,
            IsCurrent: entity.IsCurrent);
    }

    public static List<TechnicianExperienceResponse> ToDtos(
        this IEnumerable<TechnicianExperience> entities)
    {
        return entities.Select(ToTechnicianExperienceResponse).ToList();
    }
}
