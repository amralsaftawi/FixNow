using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Application.Features.TechnicianProfiles.Dtos.Responses;
using FixNow.Application.Features.TechnicianProfiles.Mappers;

namespace FixNow.Application.Features.TechnicianProfiles.Commands.AddTechnicianService;

public sealed class AddTechnicianServiceCommandHandler(
    ITechnicianProfileRepository technicianProfileRepository,
    IServiceCategoryRepository serviceCategoryRepository,
    ICurrentUser currentUser)
    : ICommandHandler<AddTechnicianServiceCommand, Result<TechnicianServiceResponse>>
{
    public async Task<Result<TechnicianServiceResponse>> Handle(
        AddTechnicianServiceCommand command,
        CancellationToken cancellationToken)
    {
        // 1. Find the current user's technician profile (with services loaded).
        var technicianProfile = await technicianProfileRepository
            .GetByUserIdWithServicesAsync(
                currentUser.UserId,
                cancellationToken);

        if (technicianProfile is null)
        {
            return TechnicianProfileErrors.NotFound;
        }

        // 2. Validate the service category exists and is active in one query.
        var categories = await serviceCategoryRepository.GetByIdsAsync(
            [command.ServiceCategoryId],
            cancellationToken);

        var category = categories.FirstOrDefault();

        if (category is null || !category.IsActive)
        {
            return ServiceCategoryErrors.NotFound;
        }

        // 3. Create the technician service.
        var serviceResult = TechnicianService.Create(
            id: Guid.NewGuid(),
            technicianProfileId: technicianProfile.Id,
            serviceCategoryId: command.ServiceCategoryId);

        if (serviceResult.IsError)
        {
            return serviceResult.Errors;
        }

        // 4. Add the service to the profile (guards against duplicates).
        var addResult = technicianProfile.AddService(
            serviceResult.Value);

        if (addResult.IsError)
        {
            return addResult.Errors;
        }

        // 5. Track the new service so it is inserted.
        await technicianProfileRepository.AddServiceAsync(
            serviceResult.Value,
            cancellationToken);

        // 6. Return the created service.
        return serviceResult.Value.ToTechnicianServiceResponse(category);
    }
}
