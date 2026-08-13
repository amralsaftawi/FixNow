using FixNow.Application.Features.TechnicianProfiles.Dtos.Responses;

namespace FixNow.Application.Features.TechnicianDiscovery.Queries.GetTechnicianPortfolio;

public sealed record TechnicianPortfolioResponse(
    IReadOnlyCollection<TechnicianPortfolioItemResponse> Items);
