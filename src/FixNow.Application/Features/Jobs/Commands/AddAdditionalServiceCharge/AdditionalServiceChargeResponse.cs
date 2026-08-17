namespace FixNow.Application.Features.Jobs.Commands.AddAdditionalServiceCharge;

public sealed record AdditionalServiceChargeResponse(
    Guid AdditionalChargeId,
    Guid JobId,
    string Description,
    Money Amount,
    DateTimeOffset CreatedAtUtc);
