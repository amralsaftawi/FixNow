
namespace FixNow.Application.Features.ServiceCategories.Commands.CreateServiceCategory;

public static class CreateServiceCategoryMapping
{
    public static CreateServiceCategoryResponse ToCreateServiceCategoryResponse(
        this ServiceCategory entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new CreateServiceCategoryResponse(
            ServiceCategoryId: entity.Id,
            Name: entity.Name,
            Description: entity.Description);
    }
}