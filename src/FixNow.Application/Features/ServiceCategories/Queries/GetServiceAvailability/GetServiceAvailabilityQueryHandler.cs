using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Domain.Common.Errors;

namespace FixNow.Application.Features.ServiceCategories.Queries.GetServiceAvailability;

public sealed class GetServiceAvailabilityQueryHandler(
    IServiceCategoryRepository serviceCategoryRepository)
    : IQueryHandler<
        GetServiceAvailabilityQuery,
        Result<ServiceAvailabilityResponse>>
{
    public async Task<Result<ServiceAvailabilityResponse>> Handle(
        GetServiceAvailabilityQuery query,
        CancellationToken cancellationToken)
    {
        var serviceCategory = await serviceCategoryRepository.GetByIdAsync(
            query.ServiceCategoryId,
            cancellationToken);

        if (serviceCategory is null)
        {
            return ServiceCategoryErrors.NotFound;
        }

        return new ServiceAvailabilityResponse(
            ServiceCategoryId: serviceCategory.Id,
            IsAvailable: serviceCategory.IsActive);
    }
}
