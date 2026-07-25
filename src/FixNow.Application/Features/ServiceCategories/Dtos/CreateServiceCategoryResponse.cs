namespace FixNow.Application.Features.ServiceCategories.Commands.CreateServiceCategory;

public sealed record CreateServiceCategoryResponse(
    Guid ServiceCategoryId,
    string Name,
    string Description);