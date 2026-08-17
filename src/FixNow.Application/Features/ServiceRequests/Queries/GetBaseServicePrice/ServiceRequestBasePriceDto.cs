namespace FixNow.Application.Features.ServiceRequests.Queries.GetBaseServicePrice;

public sealed record ServiceRequestBasePriceDto(
    Guid ServiceCategoryId,
    string ServiceCategoryName,
    Money? BasePrice,
    Money? InspectionFee);
