using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Application.Features.Jobs.Queries.GetFinalJobPrice;

namespace FixNow.Application.Common.Helpers;

internal static class FinalPriceResolver
{
    internal static async Task<Money?> ResolveAsync(
        JobFinalPriceDto pricing,
        Guid technicianProfileId,
        ITechnicianProfileRepository technicianProfileRepository,
        CancellationToken cancellationToken = default)
    {
        var servicePrice = pricing.FinalizedServicePrice;

        if (servicePrice is null)
        {
            var technicianPrice =
                await technicianProfileRepository.GetServicePriceByCategoryAsync(
                    technicianProfileId,
                    pricing.ServiceCategoryId,
                    cancellationToken);

            servicePrice = technicianPrice ?? pricing.BasePrice;
        }

        var inspectionFee = pricing.FinalizedInspectionFee ?? pricing.InspectionFee;

        Money? additionalChargesTotal = null;

        if (pricing.AdditionalChargesTotal > 0
            && pricing.AdditionalChargesCurrency is { } chargeCurrency)
        {
            var chargeAmount = Money.Create(
                pricing.AdditionalChargesTotal,
                chargeCurrency);

            if (chargeAmount.IsSuccess)
            {
                additionalChargesTotal = chargeAmount.Value;
            }
        }

        return Money.Sum(
            servicePrice,
            inspectionFee,
            additionalChargesTotal);
    }
}
