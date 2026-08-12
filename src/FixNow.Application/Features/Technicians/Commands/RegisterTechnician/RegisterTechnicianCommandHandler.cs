using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Application.Features.TechnicianProfiles.Mappers;

namespace FixNow.Application.Features.TechnicianProfiles.Commands.RegisterTechnician;

public sealed class RegisterTechnicianCommandHandler(
    ITechnicianProfileRepository technicianProfileRepository,
    IServiceCategoryRepository serviceCategoryRepository,
    ICurrentUser currentUser)
    : ICommandHandler<RegisterTechnicianCommand, Result<RegisterTechnicianResponse>>
{
    private readonly ITechnicianProfileRepository _technicianProfileRepository =
        technicianProfileRepository;

    private readonly IServiceCategoryRepository _serviceCategoryRepository =
        serviceCategoryRepository;

    private readonly ICurrentUser _currentUser = currentUser;

    public async Task<Result<RegisterTechnicianResponse>> Handle(RegisterTechnicianCommand command,CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId;

        var profileExists = await _technicianProfileRepository.ExistsByUserIdAsync(userId,cancellationToken);

        if (profileExists)
        {
            return TechnicianProfileErrors.AlreadyExists;
        }

        var nationalIdImageKey = command.NationalIdImageKey?.Trim();

        if (!string.IsNullOrWhiteSpace(nationalIdImageKey)
            && !IsOwnedByCurrentUser(
                nationalIdImageKey,
                userId))
        {
            return TechnicianProfileErrors.NationalIdImageOwnershipInvalid;
        }

        var createResult = TechnicianProfile.Create(
            id: Guid.NewGuid(),
            userId: userId,
            yearsOfExperience: command.YearsOfExperience,
            bio: command.Bio,
            nationalIdImageKey: nationalIdImageKey);

        if (createResult.IsError)
        {
            return createResult.Errors;
        }

        var technicianProfile = createResult.Value;

        var distinctServiceCategoryIds = command.ServiceCategoryIds.Distinct().ToList();

        var serviceCategories = await _serviceCategoryRepository.GetByIdsAsync(
            distinctServiceCategoryIds,
            cancellationToken);

        if (serviceCategories.Count != distinctServiceCategoryIds.Count
            || serviceCategories.Any(category => !category.IsActive))
        {
            return ServiceCategoryErrors.NotFound;
        }

        foreach (var serviceCategoryId in distinctServiceCategoryIds)
        {
            var serviceResult = TechnicianService.Create(
                id: Guid.NewGuid(),
                technicianProfileId: technicianProfile.Id,
                serviceCategoryId: serviceCategoryId);

            if (serviceResult.IsError)
            {
                return serviceResult.Errors;
            }

            var addResult = technicianProfile.AddService(serviceResult.Value);

            if (addResult.IsError)
            {
                return addResult.Errors;
            }
        }

        await _technicianProfileRepository.AddAsync(
            technicianProfile,
            cancellationToken);

        return technicianProfile.ToRegisterTechnicianResponse();
    }

    private static bool IsOwnedByCurrentUser(
        string nationalIdImageKey,
        Guid userId)
    {
        var expectedPrefix =
            $"{RegisterTechnicianCommand.NationalIdFolderPrefix}/{userId}/";

        return nationalIdImageKey.StartsWith(
            expectedPrefix,
            StringComparison.OrdinalIgnoreCase);
    }
}
