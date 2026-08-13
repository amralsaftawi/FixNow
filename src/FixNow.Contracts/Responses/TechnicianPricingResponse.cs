namespace FixNow.Contracts.Responses;

public sealed record TechnicianPricingResponse(
    IReadOnlyCollection<TechnicianServicePricingResponse> Items);
