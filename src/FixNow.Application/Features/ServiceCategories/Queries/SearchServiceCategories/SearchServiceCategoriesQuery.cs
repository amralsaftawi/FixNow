using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.ServiceCategories.Queries.SearchServiceCategories;

public sealed record SearchServiceCategoriesQuery(
    string? Search,
    int PageNumber = 1,
    int PageSize = 20)
    : IQuery<Result<SearchServiceCategoriesResponse>>;
