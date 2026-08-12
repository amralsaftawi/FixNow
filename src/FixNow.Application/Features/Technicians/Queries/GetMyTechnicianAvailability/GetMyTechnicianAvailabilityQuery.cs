using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Features.TechnicianProfiles.Dtos.Responses;

namespace FixNow.Application.Features.TechnicianProfiles.Queries.GetMyTechnicianAvailability;

public sealed record GetMyTechnicianAvailabilityQuery
    : IQuery<Result<TechnicianAvailabilitySettingsResponse>>;
