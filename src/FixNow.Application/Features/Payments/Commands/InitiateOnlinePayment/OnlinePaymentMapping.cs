namespace FixNow.Application.Features.Payments.Commands.InitiateOnlinePayment;

public static class OnlinePaymentMapping
{
    public static OnlinePaymentResponse ToOnlinePaymentResponse(
        this Payment payment,
        Guid jobId)
    {
        ArgumentNullException.ThrowIfNull(payment);

        return new OnlinePaymentResponse(
            PaymentId: payment.Id,
            JobId: jobId,
            AssignmentId: payment.AssignmentId,
            PaymentMethod: payment.PaymentMethod,
            Amount: payment.Amount,
            Status: payment.Status,
            ProviderReference: payment.ProviderReference);
    }
}
