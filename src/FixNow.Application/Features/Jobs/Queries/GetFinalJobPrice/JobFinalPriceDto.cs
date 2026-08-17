namespace FixNow.Application.Features.Jobs.Queries.GetFinalJobPrice;

public sealed record JobFinalPriceDto(
    JobStatus Status,
    Money? FinalizedServicePrice,
    Money? FinalizedInspectionFee,
    Guid ServiceCategoryId,
    Money? BasePrice,
    Money? InspectionFee,
    decimal AdditionalChargesTotal,
    Currency? AdditionalChargesCurrency);
