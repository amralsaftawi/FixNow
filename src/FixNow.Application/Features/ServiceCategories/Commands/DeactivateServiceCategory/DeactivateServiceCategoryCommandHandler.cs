using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Domain.Common.Errors;

namespace FixNow.Application.Features.ServiceCategories.Commands.DeactivateServiceCategory;

public sealed class DeactivateServiceCategoryCommandHandler(
    IServiceCategoryRepository serviceCategoryRepository)
    : ICommandHandler<DeactivateServiceCategoryCommand, Result<Updated>>
{
    private readonly IServiceCategoryRepository _serviceCategoryRepository =
        serviceCategoryRepository;

    public async Task<Result<Updated>> Handle(
        DeactivateServiceCategoryCommand command,
        CancellationToken cancellationToken)
    {
        var serviceCategory = await _serviceCategoryRepository.GetByIdAsync(
            command.ServiceCategoryId,
            cancellationToken);

        if (serviceCategory is null)
        {
            return ServiceCategoryErrors.NotFound;
        }

        var deactivateResult = serviceCategory.Deactivate();

        if (deactivateResult.IsError)
        {
            return deactivateResult.Errors;
        }

        return Result.Updated;
    }
}