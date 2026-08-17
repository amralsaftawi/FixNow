namespace FixNow.Application.Features.Jobs.Queries.GetJobTimeline;

public sealed record JobAccessDto(
    Guid ServiceRequestId,
    Guid ServiceRequestCustomerProfileId,
    Guid TechnicianProfileId);
