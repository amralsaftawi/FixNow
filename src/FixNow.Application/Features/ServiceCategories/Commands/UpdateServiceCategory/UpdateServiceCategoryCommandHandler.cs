using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Domain.Common.Errors;

namespace FixNow.Application.Features.ServiceCategories.Commands.UpdateServiceCategory;

public sealed class UpdateServiceCategoryCommandHandler(
    IServiceCategoryRepository serviceCategoryRepository)
    : ICommandHandler<UpdateServiceCategoryCommand, Result<Updated>>
{
    private readonly IServiceCategoryRepository _serviceCategoryRepository =
        serviceCategoryRepository;

    public async Task<Result<Updated>> Handle(
        UpdateServiceCategoryCommand command,
        CancellationToken cancellationToken)
    {
        var serviceCategory = await _serviceCategoryRepository.GetByIdAsync(
            command.ServiceCategoryId,
            cancellationToken);

        if (serviceCategory is null)
        {
            return ServiceCategoryErrors.NotFound;
        }

        var renameResult = serviceCategory.Rename(command.Name);

        if (renameResult.IsError)
        {
            return renameResult.Errors;
        }

        var descriptionResult = serviceCategory.ChangeDescription(
            command.Description);

        if (descriptionResult.IsError)
        {
            return descriptionResult.Errors;
        }

        var iconResult = serviceCategory.ChangeIcon(
            command.IconKey);

        if (iconResult.IsError)
        {
            return iconResult.Errors;
        }

        var displayOrderResult = serviceCategory.ChangeDisplayOrder(
            command.DisplayOrder);

        if (displayOrderResult.IsError)
        {
            return displayOrderResult.Errors;
        }

        return Result.Updated;
    }
}