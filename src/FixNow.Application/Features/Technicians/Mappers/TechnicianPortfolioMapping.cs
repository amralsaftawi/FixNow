using System.Collections.Generic;
using System.Linq;
using FixNow.Application.Features.TechnicianProfiles.Dtos.Responses;

namespace FixNow.Application.Features.TechnicianProfiles.Mappers;

public static class TechnicianPortfolioMapping
{
    public static TechnicianPortfolioItemResponse ToTechnicianPortfolioItemResponse(
        this TechnicianPortfolioItem entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new TechnicianPortfolioItemResponse(
            PortfolioItemId: entity.Id,
            TechnicianProfileId: entity.TechnicianProfileId,
            Title: entity.Title,
            Description: entity.Description,
            Media: entity.Media
                .OrderBy(media => media.DisplayOrder)
                .Select(media => new TechnicianPortfolioMediaResponse(
                    PortfolioMediaId: media.Id,
                    MediaKey: media.MediaKey,
                    DisplayOrder: media.DisplayOrder))
                .ToList());
    }

    public static List<TechnicianPortfolioItemResponse> ToDtos(
        this IEnumerable<TechnicianPortfolioItem> entities)
    {
        return entities.Select(ToTechnicianPortfolioItemResponse).ToList();
    }
}
