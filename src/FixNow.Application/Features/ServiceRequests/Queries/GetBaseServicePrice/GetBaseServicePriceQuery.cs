namespace FixNow.Application.Features.ServiceRequests.Queries.GetBaseServicePrice;

public sealed record GetBaseServicePriceQuery(
    Guid ServiceRequestId)
    : IQuery<Result<GetBaseServicePriceResponse>>;
