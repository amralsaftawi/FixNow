using FixNow.Application.Features.ServiceCategories.Dtos;

namespace FixNow.Application.Features.ServiceCategories.Queries.GetServiceCategories;

public sealed record GetServiceCategoriesResponse(
    IReadOnlyCollection<ServiceCategoryDto> Items,
    int PageNumber,
    int PageSize,
    int TotalCount,
    int TotalPages);