using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.TechnicianDiscovery.Queries.GetTechnicianRating;

public sealed record GetTechnicianRatingQuery(
    Guid TechnicianProfileId)
    : IQuery<Result<GetTechnicianRatingResponse>>;
