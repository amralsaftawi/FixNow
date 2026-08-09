using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.ServiceCategories.Queries.FilterServiceCategories;

public sealed record FilterServiceCategoriesQuery(
    string? Search,
    decimal? MinPrice,
    decimal? MaxPrice,
    ServiceCategorySortBy SortBy = ServiceCategorySortBy.Default,
    int PageNumber = 1,
    int PageSize = 20)
    : IQuery<Result<FilterServiceCategoriesResponse>>;
