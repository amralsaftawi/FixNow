using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Application.Common.Interfaces.Storage;

namespace FixNow.Application.Features.TechnicianProfiles.Commands.UploadTechnicianPortfolioMedia;

public sealed class UploadTechnicianPortfolioMediaCommandHandler(
    ITechnicianProfileRepository technicianProfileRepository,
    IFileStorage fileStorage,
    ICurrentUser currentUser)
    : ICommandHandler<
        UploadTechnicianPortfolioMediaCommand,
        Result<UploadTechnicianPortfolioMediaResponse>>
{
    private readonly ITechnicianProfileRepository _technicianProfileRepository =
        technicianProfileRepository;

    private readonly IFileStorage _fileStorage = fileStorage;

    private readonly ICurrentUser _currentUser = currentUser;

    public async Task<Result<UploadTechnicianPortfolioMediaResponse>> Handle(
        UploadTechnicianPortfolioMediaCommand command,
        CancellationToken cancellationToken)
    {
        var technicianProfile = await _technicianProfileRepository.GetByUserIdAsync(
            _currentUser.UserId,
            cancellationToken);

        if (technicianProfile is null)
        {
            return TechnicianProfileErrors.NotFound;
        }

        var key = BuildMediaKey(
            _currentUser.UserId,
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

        return new UploadTechnicianPortfolioMediaResponse(
            MediaKey: storeResult.Value);
    }

    private static string BuildMediaKey(
        Guid userId,
        string fileName)
    {
        var extension = Path.GetExtension(fileName).ToLowerInvariant();

        var storedFileName = $"{Guid.NewGuid():N}{extension}";

        return $"{UploadTechnicianPortfolioMediaCommand.PortfolioMediaFolderPrefix}/{userId}/{storedFileName}";
    }
}
