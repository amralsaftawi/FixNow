namespace FixNow.Application.Features.ServiceRequests.Queries.GetBaseServicePrice;

public sealed record GetBaseServicePriceResponse(
    Guid ServiceRequestId,
    Guid ServiceCategoryId,
    string ServiceCategoryName,
    Money? BasePrice,
    Money? InspectionFee);
