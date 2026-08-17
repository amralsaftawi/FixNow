namespace FixNow.Application.Features.Jobs.Queries.GetFinalJobPrice;

public sealed record GetFinalJobPriceQuery(
    Guid JobId)
    : IQuery<Result<GetFinalJobPriceResponse>>;
