namespace FixNow.Application.Features.Jobs.Queries.GetFinalJobPrice;

public sealed record JobPricingSourceDto(
    Guid ServiceCategoryId,
    Money? BasePrice,
    Money? InspectionFee);
