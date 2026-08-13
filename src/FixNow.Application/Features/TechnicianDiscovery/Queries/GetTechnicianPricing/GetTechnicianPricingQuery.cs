using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Features.TechnicianDiscovery.Queries.GetTechnicianPricing;

namespace FixNow.Application.Features.TechnicianDiscovery.Queries.GetTechnicianPricing;

public sealed record GetTechnicianPricingQuery(
    Guid TechnicianProfileId)
    : IQuery<Result<TechnicianPricingResponse>>;
