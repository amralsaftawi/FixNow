using FixNow.Application.Features.TechnicianProfiles.Dtos.Responses;

namespace FixNow.Application.Features.TechnicianDiscovery.Queries.GetTechnicianPricing;

public sealed record TechnicianPricingResponse(
    IReadOnlyCollection<TechnicianServicePricingResponse> Items);
