namespace FixNow.Application.Features.ServiceCategories.Queries.GetServiceCategoryById;

public sealed record GetServiceCategoryByIdResponse(
    Guid ServiceCategoryId,
    string Name,
    string Description,
    string IconKey,
    int DisplayOrder,
    bool IsActive);