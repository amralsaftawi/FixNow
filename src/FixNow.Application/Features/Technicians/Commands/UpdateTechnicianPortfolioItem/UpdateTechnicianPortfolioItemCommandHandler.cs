using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Application.Common.Interfaces.Storage;
using FixNow.Application.Features.TechnicianProfiles.Commands.UploadTechnicianPortfolioMedia;
using FixNow.Application.Features.TechnicianProfiles.Dtos.Responses;
using FixNow.Application.Features.TechnicianProfiles.Mappers;

namespace FixNow.Application.Features.TechnicianProfiles.Commands.UpdateTechnicianPortfolioItem;

public sealed class UpdateTechnicianPortfolioItemCommandHandler(
    ITechnicianProfileRepository technicianProfileRepository,
    IFileStorage fileStorage,
    ICurrentUser currentUser)
    : ICommandHandler<
        UpdateTechnicianPortfolioItemCommand,
        Result<TechnicianPortfolioItemResponse>>
{
    private readonly ITechnicianProfileRepository _technicianProfileRepository =
        technicianProfileRepository;

    private readonly IFileStorage _fileStorage = fileStorage;

    private readonly ICurrentUser _currentUser = currentUser;

    public async Task<Result<TechnicianPortfolioItemResponse>> Handle(
        UpdateTechnicianPortfolioItemCommand command,
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

        // 3. Never trust media keys owned by another user.
        if (!MediaKeysAreOwnedByCurrentUser(
                command.MediaKeys,
                _currentUser.UserId))
        {
            return TechnicianProfileErrors.PortfolioMediaOwnershipInvalid;
        }

        // 4. Capture the keys that will be replaced so their files can be cleaned up.
        var previousMediaKeys = portfolioItem.Media
            .Select(media => media.MediaKey)
            .ToList();

        // 5. Update the item.
        var updateResult = portfolioItem.Update(
            title: command.Title,
            description: command.Description,
            mediaKeys: command.MediaKeys);

        if (updateResult.IsError)
        {
            return updateResult.Errors;
        }

        // 6. Delete storage files that are no longer referenced.
        var retainedMediaKeys = command.MediaKeys
            ?.Select(key => key?.Trim())
            .Where(key => !string.IsNullOrWhiteSpace(key))
            .ToHashSet(StringComparer.Ordinal)
            ?? [];

        var removedMediaKeys = previousMediaKeys
            .Where(key => !retainedMediaKeys.Contains(key))
            .ToList();

        foreach (var removedKey in removedMediaKeys)
        {
            if (IsManagedMedia(removedKey))
            {
                await _fileStorage.DeleteAsync(
                    removedKey,
                    cancellationToken);
            }
        }

        // 7. Return the updated item.
        return portfolioItem.ToTechnicianPortfolioItemResponse();
    }

    private static bool MediaKeysAreOwnedByCurrentUser(
        IReadOnlyCollection<string>? mediaKeys,
        Guid userId)
    {
        if (mediaKeys is null)
        {
            return true;
        }

        var expectedPrefix =
            $"{UploadTechnicianPortfolioMediaCommand.PortfolioMediaFolderPrefix}/{userId}/";

        return mediaKeys.All(key =>
            string.IsNullOrWhiteSpace(key)
            || key.Trim().StartsWith(
                expectedPrefix,
                StringComparison.OrdinalIgnoreCase));
    }

    private static bool IsManagedMedia(string? mediaKey)
    {
        return !string.IsNullOrWhiteSpace(mediaKey)
            && mediaKey.StartsWith(
                $"{UploadTechnicianPortfolioMediaCommand.PortfolioMediaFolderPrefix}/",
                StringComparison.Ordinal);
    }
}
