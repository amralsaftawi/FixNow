using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.ServiceCategories.Queries.GetServiceCategories;

public sealed record GetServiceCategoriesQuery(
    string? Search,
    bool? IsActive,
    int PageNumber = 1,
    int PageSize = 20)
    : IQuery<Result<GetServiceCategoriesResponse>>;