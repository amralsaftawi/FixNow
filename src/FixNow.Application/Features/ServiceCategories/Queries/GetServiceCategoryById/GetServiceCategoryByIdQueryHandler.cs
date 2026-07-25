using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Domain.Common.Errors;

namespace FixNow.Application.Features.ServiceCategories.Queries.GetServiceCategoryById;

public sealed class GetServiceCategoryByIdQueryHandler(
    IServiceCategoryRepository serviceCategoryRepository)
    : IQueryHandler<
        GetServiceCategoryByIdQuery,
        Result<GetServiceCategoryByIdResponse>>
{
    private readonly IServiceCategoryRepository _serviceCategoryRepository =
        serviceCategoryRepository;

    public async Task<Result<GetServiceCategoryByIdResponse>> Handle(
        GetServiceCategoryByIdQuery query,
        CancellationToken cancellationToken)
    {
        var serviceCategory = await _serviceCategoryRepository.GetByIdAsync(
            query.ServiceCategoryId,
            cancellationToken);

        if (serviceCategory is null)
        {
            return ServiceCategoryErrors.NotFound;
        }

        return serviceCategory.ToGetServiceCategoryByIdResponse();
    }
}