using FixNow.Application.Common.Interfaces.Services;

namespace FixNow.Application.Features.Payments.Commands.HandlePaymentFailure;

public static class HandlePaymentFailureMapping
{
    public static HandlePaymentFailureResponse ToHandlePaymentFailureResponse(
        this PaymentFailureResult result)
    {
        return new HandlePaymentFailureResponse(
            PaymentId: result.PaymentId,
            Status: result.Status);
    }
}
