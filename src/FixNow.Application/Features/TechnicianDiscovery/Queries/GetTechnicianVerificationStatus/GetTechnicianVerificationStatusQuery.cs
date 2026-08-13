using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Features.TechnicianProfiles.Dtos.Responses;

namespace FixNow.Application.Features.TechnicianDiscovery.Queries.GetTechnicianVerificationStatus;

public sealed record GetTechnicianVerificationStatusQuery(
    Guid TechnicianProfileId)
    : IQuery<Result<TechnicianVerificationStatusResponse>>;
