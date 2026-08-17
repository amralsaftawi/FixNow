using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.Jobs.Queries.GetCustomerJobEta;

public sealed record GetCustomerJobEtaQuery(
    Guid JobId)
    : IQuery<Result<GetCustomerJobEtaResponse>>;
