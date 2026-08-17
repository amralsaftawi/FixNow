using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.Jobs.Queries.GetCustomerJobTracking;

public sealed record GetCustomerJobTrackingQuery(
    Guid JobId)
    : IQuery<Result<GetCustomerJobTrackingResponse>>;
