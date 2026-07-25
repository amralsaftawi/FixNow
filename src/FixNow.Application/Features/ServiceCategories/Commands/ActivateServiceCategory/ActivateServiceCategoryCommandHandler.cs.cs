using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Domain.Common.Errors;

namespace FixNow.Application.Features.ServiceCategories.Commands.ActivateServiceCategory;

public sealed class ActivateServiceCategoryCommandHandler(
    IServiceCategoryRepository serviceCategoryRepository)
    : ICommandHandler<ActivateServiceCategoryCommand, Result<Updated>>
{
    private readonly IServiceCategoryRepository _serviceCategoryRepository =
        serviceCategoryRepository;

    public async Task<Result<Updated>> Handle(
        ActivateServiceCategoryCommand command,
        CancellationToken cancellationToken)
    {
        var serviceCategory = await _serviceCategoryRepository.GetByIdAsync(
            command.ServiceCategoryId,
            cancellationToken);

        if (serviceCategory is null)
        {
            return ServiceCategoryErrors.NotFound;
        }

        var activateResult = serviceCategory.Activate();

        if (activateResult.IsError)
        {
            return activateResult.Errors;
        }

        return Result.Updated;
    }
}