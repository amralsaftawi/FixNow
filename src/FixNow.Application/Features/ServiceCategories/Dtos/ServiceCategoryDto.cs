namespace FixNow.Application.Features.ServiceCategories.Dtos;

public sealed record ServiceCategoryDto(
    Guid ServiceCategoryId,
    string Name,
    string Description,
    string IconKey,
    int DisplayOrder,
    bool IsActive);