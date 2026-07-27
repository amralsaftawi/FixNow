using System.Collections.Generic;
using FixNow.Application.Features.TechnicianProfiles.Dtos.Responses;

namespace FixNow.Application.Features.TechnicianProfiles.Queries.GetTechnicianProfiles;

public sealed record TechnicianProfilesResponse(
    IReadOnlyCollection<TechnicianProfileResponse> Items,
    int PageNumber,
    int PageSize,
    int TotalCount,
    int TotalPages);