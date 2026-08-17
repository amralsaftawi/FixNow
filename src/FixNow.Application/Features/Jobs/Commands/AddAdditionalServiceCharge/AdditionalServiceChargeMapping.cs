namespace FixNow.Application.Features.Jobs.Commands.AddAdditionalServiceCharge;

public static class AdditionalServiceChargeMapping
{
    public static AdditionalServiceChargeResponse ToAdditionalServiceChargeResponse(
        this JobAdditionalCharge entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new AdditionalServiceChargeResponse(
            AdditionalChargeId: entity.Id,
            JobId: entity.JobId,
            Description: entity.Description,
            Amount: entity.Amount,
            CreatedAtUtc: entity.CreatedAtUtc);
    }
}
