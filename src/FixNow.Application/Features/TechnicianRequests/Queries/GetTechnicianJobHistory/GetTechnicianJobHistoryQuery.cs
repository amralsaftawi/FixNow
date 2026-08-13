namespace FixNow.Application.Features.TechnicianRequests.Queries.GetTechnicianJobHistory;

public sealed record GetTechnicianJobHistoryQuery(
    int PageNumber,
    int PageSize)
    : IQuery<Result<GetTechnicianJobHistoryResponse>>;
