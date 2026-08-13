using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Application.Features.TechnicianDiscovery.Queries.GetTechnicianVerificationStatus;
using FixNow.Application.Features.TechnicianProfiles.Dtos.Responses;

namespace FixNow.Application.Features.TechnicianDiscovery.Queries.GetTechnicianVerificationStatus;

public sealed class GetTechnicianVerificationStatusQueryHandler(
    ITechnicianDiscoveryRepository technicianDiscoveryRepository)
    : IQueryHandler<
        GetTechnicianVerificationStatusQuery,
        Result<TechnicianVerificationStatusResponse>>
{
    private readonly ITechnicianDiscoveryRepository _technicianDiscoveryRepository =
        technicianDiscoveryRepository;

    public async Task<Result<TechnicianVerificationStatusResponse>> Handle(
        GetTechnicianVerificationStatusQuery query,
        CancellationToken cancellationToken)
    {
        var status =
            await _technicianDiscoveryRepository.GetVerificationStatusByTechnicianAsync(
                technicianProfileId: query.TechnicianProfileId,
                cancellationToken: cancellationToken);

        if (status is null)
        {
            return TechnicianProfileErrors.NotFound;
        }

        return status;
    }
}
