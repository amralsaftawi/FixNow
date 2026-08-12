using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Features.TechnicianProfiles.Dtos.Responses;

namespace FixNow.Application.Features.TechnicianProfiles.Queries.GetTechnicianProfiles;

public sealed record GetTechnicianProfilesQuery(
    VerificationStatus? VerificationStatus = null,
    int PageNumber = 1,
    int PageSize = 20)
    : IQuery<Result<TechnicianProfilesResponse>>;
