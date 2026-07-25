using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Application.Features.ServiceCategories.Dtos;

namespace FixNow.Application.Features.ServiceCategories.Queries.GetServiceCategories;

public sealed class GetServiceCategoriesQueryHandler(
    IServiceCategoryRepository serviceCategoryRepository)
    : IQueryHandler<
        GetServiceCategoriesQuery,
        Result<GetServiceCategoriesResponse>>
{
    private readonly IServiceCategoryRepository _serviceCategoryRepository =
        serviceCategoryRepository;

    public async Task<Result<GetServiceCategoriesResponse>> Handle(
        GetServiceCategoriesQuery query,
        CancellationToken cancellationToken)
    {
        var result = await _serviceCategoryRepository.GetPagedAsync(
            search: query.Search,
            isActive: query.IsActive,
            pageNumber: query.PageNumber,
            pageSize: query.PageSize,
            cancellationToken: cancellationToken);

        return new GetServiceCategoriesResponse(
            Items: result.Items.ToDtos(),
            PageNumber: result.PageNumber,
            PageSize: result.PageSize,
            TotalCount: result.TotalCount,
            TotalPages: result.TotalPages);
    }
}