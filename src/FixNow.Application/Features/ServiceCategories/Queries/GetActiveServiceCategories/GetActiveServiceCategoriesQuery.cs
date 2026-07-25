using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.ServiceCategories.Queries.GetActiveServiceCategories;

public sealed record GetActiveServiceCategoriesQuery(
    string? Search,
    int PageNumber = 1,
    int PageSize = 20)
    : IQuery<Result<GetActiveServiceCategoriesResponse>>;