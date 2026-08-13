namespace FixNow.Application.Features.TechnicianRequests.Queries.GetTechnicianActiveJobs;

public sealed record GetTechnicianActiveJobsQuery(
    int PageNumber,
    int PageSize)
    : IQuery<Result<GetTechnicianActiveJobsResponse>>;
