using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;

public sealed class CreateTechnicianProfileCommandHandler(
    ITechnicianProfileRepository technicianProfileRepository,
    ICurrentUser currentUser)
    : ICommandHandler<CreateTechnicianProfileCommand, Result<Created>>
{
    private readonly ITechnicianProfileRepository _technicianProfileRepository =
        technicianProfileRepository;

    private readonly ICurrentUser _currentUser =
        currentUser;

    public async Task<Result<Created>> Handle(
        CreateTechnicianProfileCommand command,
        CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId;

        var profileExists =await _technicianProfileRepository.ExistsByUserIdAsync( userId, cancellationToken);

        if (profileExists)
        {
            return TechnicianProfileErrors.AlreadyExists;
        }

        var createResult = TechnicianProfile.Create(
            id: Guid.NewGuid(),
            userId: userId,
            yearsOfExperience: command.YearsOfExperience,
            bio: command.Bio,
            nationalIdImageKey: command.NationalIdImageKey);

        if (createResult.IsError)
        {
            return createResult.Errors;
        }

        var technicianProfile = createResult.Value;

        await _technicianProfileRepository.AddAsync(technicianProfile,cancellationToken);

        return Result.Created;
    }
}