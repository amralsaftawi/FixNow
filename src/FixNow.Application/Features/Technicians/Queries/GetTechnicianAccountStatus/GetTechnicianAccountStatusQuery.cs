using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Features.TechnicianProfiles.Dtos.Responses;

namespace FixNow.Application.Features.TechnicianProfiles.Queries.GetTechnicianAccountStatus;

public sealed record GetTechnicianAccountStatusQuery(Guid TechnicianProfileId)
    : IQuery<Result<TechnicianAccountStatusResponse>>;
