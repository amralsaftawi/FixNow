using FixNow.Application.Features.ServiceCategories.Dtos;

namespace FixNow.Application.Features.ServiceCategories.Queries.SearchServiceCategories;

public sealed record SearchServiceCategoriesResponse(
    IReadOnlyCollection<ServiceCategoryDto> Items,
    int PageNumber,
    int PageSize,
    int TotalCount,
    int TotalPages);
