namespace FixNow.Application.Features.TechnicianProfiles.Dtos.Responses;

public sealed record TechnicianServiceResponse(
    Guid TechnicianServiceId,
    Guid TechnicianProfileId,
    Guid ServiceCategoryId,
    string ServiceCategoryName,
    string ServiceCategoryDescription,
    string? ServiceCategoryIconKey,
    int ServiceCategoryDisplayOrder,
    Money? ServiceCategoryPrice,
    bool ServiceCategoryIsActive);
