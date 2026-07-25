using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Application.Features.ServiceCategories.Mappers;

namespace FixNow.Application.Features.ServiceCategories.Queries.GetActiveServiceCategories;

public sealed class GetActiveServiceCategoriesQueryHandler(
    IServiceCategoryRepository serviceCategoryRepository)
    : IQueryHandler<
        GetActiveServiceCategoriesQuery,
        Result<GetActiveServiceCategoriesResponse>>
{
    private readonly IServiceCategoryRepository _serviceCategoryRepository =
        serviceCategoryRepository;

    public async Task<Result<GetActiveServiceCategoriesResponse>> Handle(
        GetActiveServiceCategoriesQuery query,
        CancellationToken cancellationToken)
    {
        var result = await _serviceCategoryRepository.GetActivePagedAsync(
            search: query.Search,
            pageNumber: query.PageNumber,
            pageSize: query.PageSize,
            cancellationToken: cancellationToken);

        return new GetActiveServiceCategoriesResponse(
            Items: result.Items.ToDtos(),
            PageNumber: result.PageNumber,
            PageSize: result.PageSize,
            TotalCount: result.TotalCount,
            TotalPages: result.TotalPages);
    }
}