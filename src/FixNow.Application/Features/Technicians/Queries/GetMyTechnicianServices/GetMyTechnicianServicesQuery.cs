using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Features.TechnicianProfiles.Dtos.Responses;

namespace FixNow.Application.Features.TechnicianProfiles.Queries.GetMyTechnicianServices;

public sealed record GetMyTechnicianServicesQuery
    : IQuery<Result<List<TechnicianServiceResponse>>>;
