namespace FixNow.Application.Features.Payments.Commands.CreateCashPayment;

public static class CashPaymentMapping
{
    public static CashPaymentResponse ToCashPaymentResponse(
        this Payment payment,
        Guid jobId)
    {
        ArgumentNullException.ThrowIfNull(payment);

        return new CashPaymentResponse(
            PaymentId: payment.Id,
            JobId: jobId,
            AssignmentId: payment.AssignmentId,
            PaymentMethod: payment.PaymentMethod,
            Amount: payment.Amount,
            Status: payment.Status,
            PaidAt: payment.PaidAt!.Value);
    }
}
