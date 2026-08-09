using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Application.Features.ServiceCategories.Mappers;

namespace FixNow.Application.Features.ServiceCategories.Queries.FilterServiceCategories;

public sealed class FilterServiceCategoriesQueryHandler(
    IServiceCategoryRepository serviceCategoryRepository)
    : IQueryHandler<
        FilterServiceCategoriesQuery,
        Result<FilterServiceCategoriesResponse>>
{
    private readonly IServiceCategoryRepository _serviceCategoryRepository =
        serviceCategoryRepository;

    public async Task<Result<FilterServiceCategoriesResponse>> Handle(
        FilterServiceCategoriesQuery query,
        CancellationToken cancellationToken)
    {
        var result = await _serviceCategoryRepository.FilterActivePagedAsync(
            search: query.Search,
            minPrice: query.MinPrice,
            maxPrice: query.MaxPrice,
            sortBy: query.SortBy,
            pageNumber: query.PageNumber,
            pageSize: query.PageSize,
            cancellationToken: cancellationToken);

        return new FilterServiceCategoriesResponse(
            Items: result.Items.ToDtos(),
            PageNumber: result.PageNumber,
            PageSize: result.PageSize,
            TotalCount: result.TotalCount,
            TotalPages: result.TotalPages);
    }
}
