using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Features.TechnicianProfiles.Dtos.Responses;

namespace FixNow.Application.Features.TechnicianProfiles.Queries.GetMyTechnicianPortfolioItem;

public sealed record GetMyTechnicianPortfolioItemQuery(
    Guid PortfolioItemId)
    : IQuery<Result<TechnicianPortfolioItemResponse>>;
