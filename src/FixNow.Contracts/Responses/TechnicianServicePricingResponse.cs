namespace FixNow.Contracts.Responses;

public sealed record TechnicianServicePricingResponse(
    Guid TechnicianServiceId,
    Guid TechnicianProfileId,
    Guid ServiceCategoryId,
    string ServiceCategoryName,
    Money? Price);
