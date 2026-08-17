using System.ComponentModel.DataAnnotations;

namespace FixNow.Contracts.Requests;

public sealed record AddCustomerPaymentMethodRequest
{
    [EnumDataType(
        typeof(PaymentMethod),
        ErrorMessage = "Payment method type is invalid.")]
    public PaymentMethod Type { get; init; }
}
