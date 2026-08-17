using FixNow.Application.Common.Interfaces.Services;

namespace FixNow.Application.Features.Payments.Commands.ConfirmPayment;

public static class ConfirmPaymentMapping
{
    public static ConfirmPaymentResponse ToConfirmPaymentResponse(
        this PaymentConfirmationResult result)
    {
        return new ConfirmPaymentResponse(
            PaymentId: result.PaymentId,
            Status: result.Status,
            PaidAt: result.PaidAt,
            ProviderReference: result.ProviderReference);
    }
}
