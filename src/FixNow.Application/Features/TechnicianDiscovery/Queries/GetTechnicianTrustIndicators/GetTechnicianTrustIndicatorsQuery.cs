using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.TechnicianDiscovery.Queries.GetTechnicianTrustIndicators;

public sealed record GetTechnicianTrustIndicatorsQuery(
    Guid TechnicianProfileId)
    : IQuery<Result<GetTechnicianTrustIndicatorsResponse>>;
