namespace FixNow.Application.Features.Payments.Commands.RefundPayment;

public static class RefundPaymentMapping
{
    public static RefundPaymentResponse ToRefundPaymentResponse(
        this Payment payment)
    {
        ArgumentNullException.ThrowIfNull(payment);

        return new RefundPaymentResponse(
            PaymentId: payment.Id,
            Status: payment.Status);
    }
}
