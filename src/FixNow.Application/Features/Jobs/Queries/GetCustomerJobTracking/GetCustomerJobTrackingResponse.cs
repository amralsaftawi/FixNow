namespace FixNow.Application.Features.Jobs.Queries.GetCustomerJobTracking;

public sealed record GetCustomerJobTrackingResponse(
    Guid JobId,
    JobStatus Status,
    decimal? Latitude,
    decimal? Longitude);
