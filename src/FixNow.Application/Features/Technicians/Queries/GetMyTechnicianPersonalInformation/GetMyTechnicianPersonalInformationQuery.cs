using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Features.TechnicianProfiles.Dtos.Responses;

namespace FixNow.Application.Features.TechnicianProfiles.Queries.GetMyTechnicianPersonalInformation;

public sealed record GetMyTechnicianPersonalInformationQuery
    : IQuery<Result<TechnicianPersonalInformationResponse>>;
