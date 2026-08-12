namespace FixNow.Application.Features.TechnicianProfiles.Dtos.Responses;

public sealed record TechnicianServicePricingResponse(
    Guid TechnicianServiceId,
    Guid TechnicianProfileId,
    Guid ServiceCategoryId,
    string ServiceCategoryName,
    Money? Price);
