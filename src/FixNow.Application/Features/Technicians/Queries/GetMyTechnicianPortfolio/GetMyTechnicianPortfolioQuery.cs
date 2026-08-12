using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Features.TechnicianProfiles.Dtos.Responses;

namespace FixNow.Application.Features.TechnicianProfiles.Queries.GetMyTechnicianPortfolio;

public sealed record GetMyTechnicianPortfolioQuery
    : IQuery<Result<List<TechnicianPortfolioItemResponse>>>;
