using FixNow.Application.Features.ServiceCategories.Dtos;

namespace FixNow.Application.Features.ServiceCategories.Queries.GetActiveServiceCategories;

public sealed record GetActiveServiceCategoriesResponse(
    IReadOnlyCollection<ServiceCategoryDto> Items,
    int PageNumber,
    int PageSize,
    int TotalCount,
    int TotalPages);