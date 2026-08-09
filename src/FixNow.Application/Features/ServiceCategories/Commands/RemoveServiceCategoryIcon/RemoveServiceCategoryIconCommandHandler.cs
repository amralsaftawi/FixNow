using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Application.Common.Interfaces.Storage;
using FixNow.Application.Features.ServiceCategories.Commands.UploadServiceCategoryIcon;
using FixNow.Domain.Common.Errors;

namespace FixNow.Application.Features.ServiceCategories.Commands.RemoveServiceCategoryIcon;

public sealed class RemoveServiceCategoryIconCommandHandler(
    IServiceCategoryRepository serviceCategoryRepository,
    IFileStorage fileStorage)
    : ICommandHandler<RemoveServiceCategoryIconCommand, Result<Updated>>
{
    private readonly IServiceCategoryRepository _serviceCategoryRepository =
        serviceCategoryRepository;

    private readonly IFileStorage _fileStorage = fileStorage;

    public async Task<Result<Updated>> Handle(
        RemoveServiceCategoryIconCommand command,
        CancellationToken cancellationToken)
    {
        var serviceCategory = await _serviceCategoryRepository.GetByIdAsync(
            command.ServiceCategoryId,
            cancellationToken);

        if (serviceCategory is null)
        {
            return ServiceCategoryErrors.NotFound;
        }

        var previousIconKey = serviceCategory.IconKey;

        var removeIconResult = serviceCategory.RemoveIcon();

        if (removeIconResult.IsError)
        {
            return removeIconResult.Errors;
        }

        if (IsManagedIcon(previousIconKey))
        {
            await _fileStorage.DeleteAsync(
                previousIconKey!,
                cancellationToken);
        }

        return Result.Updated;
    }

    private static bool IsManagedIcon(string? iconKey)
    {
        return !string.IsNullOrWhiteSpace(iconKey)
            && iconKey.StartsWith(
                $"{UploadServiceCategoryIconCommand.IconFolderPrefix}/",
                StringComparison.Ordinal);
    }
}
