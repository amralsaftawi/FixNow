namespace FixNow.Application.Features.Jobs.Queries.GetFinalJobPrice;

public sealed record GetFinalJobPriceResponse(
    Guid JobId,
    JobStatus Status,
    bool IsFinalized,
    Money? ServicePrice,
    Money? InspectionFee,
    Money? AdditionalChargesTotal,
    Money? FinalPrice);
