namespace FixNow.Contracts.Responses;

public sealed record TechnicianDiscoveryServiceResponse(
    Guid ServiceCategoryId,
    string ServiceCategoryName,
    string ServiceCategoryDescription,
    string? ServiceCategoryIconKey,
    int DisplayOrder);

public sealed record TechnicianServicesResponse(
    IReadOnlyCollection<TechnicianDiscoveryServiceResponse> Items);
