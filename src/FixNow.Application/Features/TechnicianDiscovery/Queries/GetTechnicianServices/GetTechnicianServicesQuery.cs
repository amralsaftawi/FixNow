using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Features.TechnicianDiscovery.Queries.GetTechnicianServices;

namespace FixNow.Application.Features.TechnicianDiscovery.Queries.GetTechnicianServices;

public sealed record GetTechnicianServicesQuery(
    Guid TechnicianProfileId)
    : IQuery<Result<TechnicianServicesResponse>>;
