using System.Collections.Generic;
using System.Linq;
using FixNow.Application.Features.TechnicianProfiles.Dtos.Responses;

namespace FixNow.Application.Features.TechnicianProfiles.Mappers;

public static class TechnicianServicePricingMapping
{
    public static TechnicianServicePricingResponse ToTechnicianServicePricingResponse(
        this TechnicianService entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new TechnicianServicePricingResponse(
            TechnicianServiceId: entity.Id,
            TechnicianProfileId: entity.TechnicianProfileId,
            ServiceCategoryId: entity.ServiceCategoryId,
            ServiceCategoryName: entity.ServiceCategory.Name,
            Price: entity.Price);
    }

    public static List<TechnicianServicePricingResponse> ToPricingDtos(
        this IEnumerable<TechnicianService> entities)
    {
        return entities.Select(ToTechnicianServicePricingResponse).ToList();
    }
}
