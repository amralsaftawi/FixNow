using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Features.TechnicianDiscovery.Queries.GetTechnicianPortfolio;

namespace FixNow.Application.Features.TechnicianDiscovery.Queries.GetTechnicianPortfolio;

public sealed record GetTechnicianPortfolioQuery(
    Guid TechnicianProfileId)
    : IQuery<Result<TechnicianPortfolioResponse>>;
