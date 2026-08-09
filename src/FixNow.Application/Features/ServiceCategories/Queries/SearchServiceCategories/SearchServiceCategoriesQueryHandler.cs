using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Application.Features.ServiceCategories.Mappers;

namespace FixNow.Application.Features.ServiceCategories.Queries.SearchServiceCategories;

public sealed class SearchServiceCategoriesQueryHandler(
    IServiceCategoryRepository serviceCategoryRepository)
    : IQueryHandler<
        SearchServiceCategoriesQuery,
        Result<SearchServiceCategoriesResponse>>
{
    private readonly IServiceCategoryRepository _serviceCategoryRepository =
        serviceCategoryRepository;

    public async Task<Result<SearchServiceCategoriesResponse>> Handle(
        SearchServiceCategoriesQuery query,
        CancellationToken cancellationToken)
    {
        var result = await _serviceCategoryRepository.SearchActivePagedAsync(
            search: query.Search,
            pageNumber: query.PageNumber,
            pageSize: query.PageSize,
            cancellationToken: cancellationToken);

        return new SearchServiceCategoriesResponse(
            Items: result.Items.ToDtos(),
            PageNumber: result.PageNumber,
            PageSize: result.PageSize,
            TotalCount: result.TotalCount,
            TotalPages: result.TotalPages);
    }
}
