using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.Identity.Queries.GetCurrentUser;

public sealed record GetCurrentUserQuery
    : IQuery<Result<GetCurrentUserResponse>>;