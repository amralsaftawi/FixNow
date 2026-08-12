using System.Collections.Generic;
using System.Linq;
using FixNow.Application.Features.TechnicianProfiles.Dtos.Responses;

namespace FixNow.Application.Features.TechnicianProfiles.Mappers;

public static class TechnicianServiceMapping
{
    public static TechnicianServiceResponse ToTechnicianServiceResponse(
        this TechnicianService entity,
        ServiceCategory category)
    {
        ArgumentNullException.ThrowIfNull(entity);
        ArgumentNullException.ThrowIfNull(category);

        return new TechnicianServiceResponse(
            TechnicianServiceId: entity.Id,
            TechnicianProfileId: entity.TechnicianProfileId,
            ServiceCategoryId: category.Id,
            ServiceCategoryName: category.Name,
            ServiceCategoryDescription: category.Description,
            ServiceCategoryIconKey: category.IconKey,
            ServiceCategoryDisplayOrder: category.DisplayOrder,
            ServiceCategoryPrice: category.Price,
            ServiceCategoryIsActive: category.IsActive);
    }

    public static TechnicianServiceResponse ToTechnicianServiceResponse(
        this TechnicianService entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return entity.ToTechnicianServiceResponse(entity.ServiceCategory);
    }

    public static List<TechnicianServiceResponse> ToDtos(
        this IEnumerable<TechnicianService> entities)
    {
        return entities.Select(ToTechnicianServiceResponse).ToList();
    }
}
