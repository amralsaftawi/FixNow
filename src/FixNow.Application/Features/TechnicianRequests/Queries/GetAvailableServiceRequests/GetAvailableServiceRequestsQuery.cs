namespace FixNow.Application.Features.TechnicianRequests.Queries.GetAvailableServiceRequests;

public sealed record GetAvailableServiceRequestsQuery(
    int PageNumber,
    int PageSize)
    : IQuery<Result<GetAvailableServiceRequestsResponse>>;
