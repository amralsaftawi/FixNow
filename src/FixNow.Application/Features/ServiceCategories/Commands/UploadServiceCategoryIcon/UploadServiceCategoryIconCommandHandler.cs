using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Application.Common.Interfaces.Storage;
using FixNow.Domain.Common.Errors;

namespace FixNow.Application.Features.ServiceCategories.Commands.UploadServiceCategoryIcon;

public sealed class UploadServiceCategoryIconCommandHandler(
    IServiceCategoryRepository serviceCategoryRepository,
    IFileStorage fileStorage)
    : ICommandHandler<
        UploadServiceCategoryIconCommand,
        Result<UploadServiceCategoryIconResponse>>
{
    private readonly IServiceCategoryRepository _serviceCategoryRepository =
        serviceCategoryRepository;

    private readonly IFileStorage _fileStorage = fileStorage;

    public async Task<Result<UploadServiceCategoryIconResponse>> Handle(
        UploadServiceCategoryIconCommand command,
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

        var key = BuildIconKey(
            command.ServiceCategoryId,
            command.FileName);

        var storeResult = await _fileStorage.StoreAsync(
            key,
            command.Content,
            command.ContentType,
            cancellationToken);

        if (storeResult.IsError)
        {
            return storeResult.Errors;
        }

        var changeIconResult = serviceCategory.ChangeIcon(key);

        if (changeIconResult.IsError)
        {
            await _fileStorage.DeleteAsync(
                key,
                cancellationToken);

            return changeIconResult.Errors;
        }

        if (IsManagedIcon(previousIconKey))
        {
            await _fileStorage.DeleteAsync(
                previousIconKey!,
                cancellationToken);
        }

        return serviceCategory.ToUploadServiceCategoryIconResponse();
    }

    private static string BuildIconKey(
        Guid serviceCategoryId,
        string fileName)
    {
        var extension = Path.GetExtension(fileName).ToLowerInvariant();

        var storedFileName = $"{Guid.NewGuid():N}{extension}";

        return $"{UploadServiceCategoryIconCommand.IconFolderPrefix}/{serviceCategoryId}/icon/{storedFileName}";
    }

    private static bool IsManagedIcon(string? iconKey)
    {
        return !string.IsNullOrWhiteSpace(iconKey)
            && iconKey.StartsWith(
                $"{UploadServiceCategoryIconCommand.IconFolderPrefix}/",
                StringComparison.Ordinal);
    }
}
