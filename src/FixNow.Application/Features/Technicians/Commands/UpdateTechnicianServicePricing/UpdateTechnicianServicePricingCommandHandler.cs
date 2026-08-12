using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Application.Features.TechnicianProfiles.Dtos.Responses;
using FixNow.Application.Features.TechnicianProfiles.Mappers;

namespace FixNow.Application.Features.TechnicianProfiles.Commands.UpdateTechnicianServicePricing;

public sealed class UpdateTechnicianServicePricingCommandHandler(
    ITechnicianProfileRepository technicianProfileRepository,
    ICurrentUser currentUser)
    : ICommandHandler<UpdateTechnicianServicePricingCommand, Result<TechnicianServicePricingResponse>>
{
    public async Task<Result<TechnicianServicePricingResponse>> Handle(
        UpdateTechnicianServicePricingCommand command,
        CancellationToken cancellationToken)
    {
        // 1. Find the current user's technician profile (with services loaded).
        //    Ownership is derived from the authenticated user, never from the client.
        var technicianProfile = await technicianProfileRepository
            .GetByUserIdWithServicesAsync(
                currentUser.UserId,
                cancellationToken);

        if (technicianProfile is null)
        {
            return TechnicianProfileErrors.NotFound;
        }

        // 2. The service must exist, belong to this profile, and therefore
        //    correspond to a service category the technician has selected.
        var service = technicianProfile.Services
            .FirstOrDefault(x => x.Id == command.TechnicianServiceId);

        if (service is null)
        {
            return TechnicianProfileErrors.ServiceNotFound;
        }

        // 3. Build a valid price using the domain monetary rules.
        var priceResult = Money.Create(
            command.Amount,
            command.Currency);

        if (priceResult.IsError)
        {
            return priceResult.Errors;
        }

        // 4. Update the price for this technician's service only.
        var setPriceResult = service.SetPrice(priceResult.Value);

        if (setPriceResult.IsError)
        {
            return setPriceResult.Errors;
        }

        // 5. Return the updated pricing.
        return service.ToTechnicianServicePricingResponse();
    }
}
