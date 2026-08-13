using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Features.TechnicianDiscovery.Queries.GetTechnicianAvailability;
using FixNow.Application.Features.TechnicianProfiles.Dtos.Responses;

namespace FixNow.Application.Features.TechnicianDiscovery.Queries.GetTechnicianAvailability;

public sealed record GetTechnicianAvailabilityQuery(
    Guid TechnicianProfileId)
    : IQuery<Result<TechnicianAvailabilitySettingsResponse>>;
