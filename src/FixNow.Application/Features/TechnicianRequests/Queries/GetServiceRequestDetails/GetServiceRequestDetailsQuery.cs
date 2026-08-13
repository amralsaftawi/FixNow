namespace FixNow.Application.Features.TechnicianRequests.Queries.GetServiceRequestDetails;

public sealed record GetServiceRequestDetailsQuery(
    Guid ServiceRequestId)
    : IQuery<Result<GetServiceRequestDetailsResponse>>;
