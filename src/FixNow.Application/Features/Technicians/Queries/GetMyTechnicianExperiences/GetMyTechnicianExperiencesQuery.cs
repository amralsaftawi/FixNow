using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Features.TechnicianProfiles.Dtos.Responses;

namespace FixNow.Application.Features.TechnicianProfiles.Queries.GetMyTechnicianExperiences;

public sealed record GetMyTechnicianExperiencesQuery
    : IQuery<Result<List<TechnicianExperienceResponse>>>;
