using FixNow.Application.Features.ServiceCategories.Dtos;

namespace FixNow.Application.Features.ServiceCategories.Queries.FilterServiceCategories;

public sealed record FilterServiceCategoriesResponse(
    IReadOnlyCollection<ServiceCategoryDto> Items,
    int PageNumber,
    int PageSize,
    int TotalCount,
    int TotalPages);
