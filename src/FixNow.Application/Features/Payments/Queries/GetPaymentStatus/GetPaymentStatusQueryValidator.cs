using FluentValidation;

namespace FixNow.Application.Features.Payments.Queries.GetPaymentStatus;

public sealed class GetPaymentStatusQueryValidator
    : AbstractValidator<GetPaymentStatusQuery>
{
    public GetPaymentStatusQueryValidator()
    {
        RuleFor(x => x.PaymentId)
            .NotEmpty()
            .WithErrorCode("Payment.IdRequired");
    }
}
