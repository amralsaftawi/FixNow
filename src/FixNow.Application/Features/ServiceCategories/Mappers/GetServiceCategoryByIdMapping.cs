

namespace FixNow.Application.Features.ServiceCategories.Queries.GetServiceCategoryById;

public static class GetServiceCategoryByIdMapping
{
    public static GetServiceCategoryByIdResponse ToGetServiceCategoryByIdResponse(
        this ServiceCategory entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new GetServiceCategoryByIdResponse(
            ServiceCategoryId: entity.Id,
            Name: entity.Name,
            Description: entity.Description,
            IconKey: entity.IconKey!,
            DisplayOrder: entity.DisplayOrder,
            IsActive: entity.IsActive);
    }
}