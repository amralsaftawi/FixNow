using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Features.CustomerProfiles.Dtos.Responses;

namespace FixNow.Application.Features.CustomerProfiles.Queries.GetMyCurrentLocation;

public sealed record GetMyCurrentLocationQuery
    : IQuery<Result<CurrentLocationResponse>>;
