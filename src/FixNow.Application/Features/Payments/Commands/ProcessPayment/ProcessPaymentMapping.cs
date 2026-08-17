namespace FixNow.Application.Features.Payments.Commands.ProcessPayment;

public static class ProcessPaymentMapping
{
    public static ProcessPaymentResponse ToProcessPaymentResponse(
        this Payment payment)
    {
        ArgumentNullException.ThrowIfNull(payment);

        return new ProcessPaymentResponse(
            PaymentId: payment.Id,
            Status: payment.Status,
            ProviderReference: payment.ProviderReference);
    }
}
