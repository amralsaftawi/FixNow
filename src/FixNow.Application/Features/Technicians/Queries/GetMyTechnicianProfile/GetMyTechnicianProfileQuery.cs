using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Features.TechnicianProfiles.Dtos.Responses;

namespace FixNow.Application.Features.TechnicianProfiles.Queries.GetMyTechnicianProfile;

public sealed record GetMyTechnicianProfileQuery
    : IQuery<Result<TechnicianProfileResponse>>;
