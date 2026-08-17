using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Helpers;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Application.Common.Interfaces.Services;
using FixNow.Application.Features.Jobs.Queries.GetFinalJobPrice;

namespace FixNow.Application.Features.Payments.Commands.InitiateOnlinePayment;

public sealed class InitiateOnlinePaymentCommandHandler(
    ICustomerRepository customerRepository,
    ITechnicianProfileRepository technicianProfileRepository,
    IJobRepository jobRepository,
    IAssignmentRepository assignmentRepository,
    IPaymentRepository paymentRepository,
    IOnlinePaymentProvider onlinePaymentProvider,
    ICurrentUser currentUser)
    : ICommandHandler<InitiateOnlinePaymentCommand, Result<OnlinePaymentResponse>>
{
    public async Task<Result<OnlinePaymentResponse>> Handle(
        InitiateOnlinePaymentCommand command,
        CancellationToken cancellationToken)
    {
        // 1. Resolve the authenticated user's customer profile. Online
        //    payment is a customer-only action: the customer who owns the
        //    job initiates the payment.
        var customerProfile = await customerRepository.GetByUserIdAsync(
            currentUser.UserId,
            cancellationToken);

        if (customerProfile is null)
        {
            return CustomerProfileErrors.NotFound;
        }

        // 2. Verify the job exists and belongs to the authenticated
        //    customer. An un-owned job is indistinguishable from a
        //    non-existent one, so job existence is never leaked.
        var jobAccess = await jobRepository.GetAccessAsync(
            command.JobId,
            cancellationToken);

        if (jobAccess is null ||
            jobAccess.ServiceRequestCustomerProfileId != customerProfile.Id)
        {
            return JobErrors.NotFound;
        }

        // 3. The accepted assignment must exist for the job's service
        //    request. The assignment anchors the Payment aggregate.
        var assignment = await assignmentRepository.GetAcceptedByRequestAsync(
            jobAccess.ServiceRequestId,
            cancellationToken);

        if (assignment is null)
        {
            return JobErrors.NotFound;
        }

        // 4. An online payment can only be initiated for a completed job.
        //    Only a completed job has an authoritative final price snapshot.
        var pricing = await jobRepository.GetFinalJobPriceAsync(
            command.JobId,
            cancellationToken);

        if (pricing is null)
        {
            return JobErrors.NotFound;
        }

        if (pricing.Status != JobStatus.Completed)
        {
            return PaymentErrors.JobNotCompleted;
        }

        // 5. Resolve the final price using the same authoritative
        //    computation that GET /api/jobs/{jobId}/price uses. This
        //    guarantees the payment amount always equals the displayed
        //    final price.
        var finalPrice = await FinalPriceResolver.ResolveAsync(
            pricing,
            jobAccess.TechnicianProfileId,
            technicianProfileRepository,
            cancellationToken);

        if (finalPrice is null)
        {
            return PaymentErrors.FinalPriceNotResolved;
        }

        // 6. Check for an existing active (non-failed) payment on this
        //    assignment. Online payments allow retries after failure, so
        //    only block if a Pending or Paid payment already exists. A
        //    Failed payment does not block a new attempt.
        var activePayment = await paymentRepository.GetActiveByAssignmentIdAsync(
            assignment.Id,
            cancellationToken);

        if (activePayment is not null)
        {
            if (activePayment.Status == PaymentStatus.Paid)
            {
                return PaymentErrors.AlreadyPaid;
            }

            // An existing Pending payment: return it rather than creating
            // a duplicate. The customer can poll this payment for status.
            return activePayment.ToOnlinePaymentResponse(command.JobId);
        }

        // 7. Create the Payment aggregate anchored to the job's accepted
        //    assignment. The amount is entirely server-derived from the
        //    final price computation above; the client supplies nothing
        //    beyond the job identifier.
        var paymentResult = Payment.Create(
            Guid.NewGuid(),
            assignment.Id,
            jobAccess.ServiceRequestCustomerProfileId,
            finalPrice,
            PaymentMethod.Card);

        if (paymentResult.IsError)
        {
            return paymentResult.Errors;
        }

        // 8. Prepare the provider interaction. The provider abstraction
        //    is provider-agnostic: a real implementation would redirect
        //    to a checkout page or return a client token. When the
        //    provider is not configured, this returns a clear failure
        //    instead of a fake success.
        var initiationResult = await onlinePaymentProvider.InitiateAsync(
            paymentResult.Value.Id,
            finalPrice,
            finalPrice.Currency.ToString(),
            cancellationToken);

        if (initiationResult.IsError)
        {
            return initiationResult.Errors;
        }

        // 9. Persist the Pending payment. The payment is NOT marked as
        //    Paid: that happens only after a real provider confirms the
        //    transaction in a future Payment Confirmation feature.
        paymentResult.Value.SetProviderReference(
            initiationResult.Value.ProviderReference);

        await paymentRepository.AddAsync(
            paymentResult.Value,
            cancellationToken);

        return paymentResult.Value.ToOnlinePaymentResponse(command.JobId);
    }
}
