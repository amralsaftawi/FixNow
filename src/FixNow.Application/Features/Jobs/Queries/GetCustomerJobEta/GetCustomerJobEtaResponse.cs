namespace FixNow.Application.Features.Jobs.Queries.GetCustomerJobEta;

public sealed record GetCustomerJobEtaResponse(
    Guid JobId,
    JobStatus Status,
    bool IsEstimateAvailable,
    DateTimeOffset? EstimatedArrivalTimeUtc,
    int? EstimatedTravelMinutes,
    double? DistanceKm);
