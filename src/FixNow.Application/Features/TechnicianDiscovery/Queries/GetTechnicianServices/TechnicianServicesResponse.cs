namespace FixNow.Application.Features.TechnicianDiscovery.Queries.GetTechnicianServices;

public sealed record TechnicianServiceDto(
    Guid ServiceCategoryId,
    string ServiceCategoryName,
    string ServiceCategoryDescription,
    string? ServiceCategoryIconKey,
    int DisplayOrder);

public sealed record TechnicianServicesResponse(
    IReadOnlyCollection<TechnicianServiceDto> Items);
