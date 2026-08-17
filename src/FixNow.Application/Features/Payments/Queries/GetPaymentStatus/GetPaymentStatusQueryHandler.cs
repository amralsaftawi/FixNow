using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;

namespace FixNow.Application.Features.Payments.Queries.GetPaymentStatus;

public sealed class GetPaymentStatusQueryHandler(
    ICustomerRepository customerRepository,
    IPaymentRepository paymentRepository,
    ICurrentUser currentUser)
    : IQueryHandler<GetPaymentStatusQuery, Result<GetPaymentStatusResponse>>
{
    public async Task<Result<GetPaymentStatusResponse>> Handle(
        GetPaymentStatusQuery query,
        CancellationToken cancellationToken)
    {
        // 1. Resolve the authenticated user's customer profile. Payment
        //    status is accessible to the customer who owns the payment.
        var customerProfile = await customerRepository.GetByUserIdAsync(
            currentUser.UserId,
            cancellationToken);

        if (customerProfile is null)
        {
            return CustomerProfileErrors.NotFound;
        }

        // 2. Load the existing payment by id. If the payment does not
        //    exist or belongs to a different customer, the same NotFound
        //    error is returned to avoid leaking payment existence.
        var payment = await paymentRepository.GetByIdAsync(
            query.PaymentId,
            cancellationToken);

        if (payment is null ||
            payment.CustomerProfileId != customerProfile.Id)
        {
            return PaymentErrors.NotFound;
        }

        // 3. Return the current payment lifecycle state. No status
        //    transition occurs. The response reflects the authoritative
        //    state persisted by the Payment aggregate.
        return new GetPaymentStatusResponse(
            PaymentId: payment.Id,
            Status: payment.Status,
            PaymentMethod: payment.PaymentMethod,
            Amount: payment.Amount,
            PaidAt: payment.PaidAt);
    }
}
