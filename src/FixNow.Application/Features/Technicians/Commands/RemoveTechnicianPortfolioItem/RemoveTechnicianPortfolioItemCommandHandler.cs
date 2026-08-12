using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Application.Common.Interfaces.Storage;
using FixNow.Application.Features.TechnicianProfiles.Commands.UploadTechnicianPortfolioMedia;

namespace FixNow.Application.Features.TechnicianProfiles.Commands.RemoveTechnicianPortfolioItem;

public sealed class RemoveTechnicianPortfolioItemCommandHandler(
    ITechnicianProfileRepository technicianProfileRepository,
    IFileStorage fileStorage,
    ICurrentUser currentUser)
    : ICommandHandler<RemoveTechnicianPortfolioItemCommand, Result<Success>>
{
    private readonly ITechnicianProfileRepository _technicianProfileRepository =
        technicianProfileRepository;

    private readonly IFileStorage _fileStorage = fileStorage;

    private readonly ICurrentUser _currentUser = currentUser;

    public async Task<Result<Success>> Handle(
        RemoveTechnicianPortfolioItemCommand command,
        CancellationToken cancellationToken)
    {
        // 1. Resolve the current user's technician profile.
        var technicianProfile = await _technicianProfileRepository
            .GetByUserIdWithPortfolioAsync(
                _currentUser.UserId,
                cancellationToken);

        if (technicianProfile is null)
        {
            return TechnicianProfileErrors.NotFound;
        }

        // 2. The item must belong to the current user's profile.
        var portfolioItem = technicianProfile.PortfolioItems
            .FirstOrDefault(item => item.Id == command.PortfolioItemId);

        if (portfolioItem is null)
        {
            return TechnicianProfileErrors.PortfolioItemNotFound;
        }

        // 3. Capture the media keys before the item is removed.
        var mediaKeys = portfolioItem.Media
            .Select(media => media.MediaKey)
            .ToList();

        // 4. Remove the item from the profile.
        var removeResult = technicianProfile.RemovePortfolioItem(
            command.PortfolioItemId);

        if (removeResult.IsError)
        {
            return removeResult.Errors;
        }

        // 5. Track the item for deletion (cascades to its media rows).
        _technicianProfileRepository.RemovePortfolioItem(portfolioItem);

        // 6. Clean up the associated storage files.
        foreach (var mediaKey in mediaKeys)
        {
            if (IsManagedMedia(mediaKey))
            {
                await _fileStorage.DeleteAsync(
                    mediaKey,
                    cancellationToken);
            }
        }

        return Result.Success;
    }

    private static bool IsManagedMedia(string? mediaKey)
    {
        return !string.IsNullOrWhiteSpace(mediaKey)
            && mediaKey.StartsWith(
                $"{UploadTechnicianPortfolioMediaCommand.PortfolioMediaFolderPrefix}/",
                StringComparison.Ordinal);
    }
}
