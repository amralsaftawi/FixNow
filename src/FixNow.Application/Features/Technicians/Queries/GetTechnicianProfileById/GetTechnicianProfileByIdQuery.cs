using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Features.TechnicianProfiles.Dtos.Responses;

namespace FixNow.Application.Features.TechnicianProfiles.Queries.GetTechnicianProfileById;

public sealed record GetTechnicianProfileByIdQuery(
    Guid TechnicianProfileId)
    : IQuery<Result<TechnicianProfileResponse>>;