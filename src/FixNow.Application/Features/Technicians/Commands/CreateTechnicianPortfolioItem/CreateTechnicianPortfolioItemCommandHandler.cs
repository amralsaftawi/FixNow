using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Application.Features.TechnicianProfiles.Commands.UploadTechnicianPortfolioMedia;
using FixNow.Application.Features.TechnicianProfiles.Dtos.Responses;
using FixNow.Application.Features.TechnicianProfiles.Mappers;

namespace FixNow.Application.Features.TechnicianProfiles.Commands.CreateTechnicianPortfolioItem;

public sealed class CreateTechnicianPortfolioItemCommandHandler(
    ITechnicianProfileRepository technicianProfileRepository,
    ICurrentUser currentUser)
    : ICommandHandler<
        CreateTechnicianPortfolioItemCommand,
        Result<TechnicianPortfolioItemResponse>>
{
    private readonly ITechnicianProfileRepository _technicianProfileRepository =
        technicianProfileRepository;

    private readonly ICurrentUser _currentUser = currentUser;

    public async Task<Result<TechnicianPortfolioItemResponse>> Handle(
        CreateTechnicianPortfolioItemCommand command,
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

        // 2. Never trust media keys owned by another user.
        if (!MediaKeysAreOwnedByCurrentUser(
                command.MediaKeys,
                _currentUser.UserId))
        {
            return TechnicianProfileErrors.PortfolioMediaOwnershipInvalid;
        }

        // 3. Create the portfolio item.
        var portfolioItemResult = TechnicianPortfolioItem.Create(
            id: Guid.NewGuid(),
            technicianProfileId: technicianProfile.Id,
            title: command.Title,
            description: command.Description,
            mediaKeys: command.MediaKeys);

        if (portfolioItemResult.IsError)
        {
            return portfolioItemResult.Errors;
        }

        // 4. Attach it to the current user's profile only.
        var addResult = technicianProfile.AddPortfolioItem(
            portfolioItemResult.Value);

        if (addResult.IsError)
        {
            return addResult.Errors;
        }

        // 5. Track the new item so it is inserted.
        await _technicianProfileRepository.AddPortfolioItemAsync(
            portfolioItemResult.Value,
            cancellationToken);

        // 6. Return the created item.
        return portfolioItemResult.Value.ToTechnicianPortfolioItemResponse();
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
}
