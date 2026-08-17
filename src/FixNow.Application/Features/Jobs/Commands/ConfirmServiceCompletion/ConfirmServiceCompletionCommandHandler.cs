using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;

namespace FixNow.Application.Features.Jobs.Commands.ConfirmServiceCompletion;

public sealed class ConfirmServiceCompletionCommandHandler(
    ICustomerRepository customerRepository,
    IJobRepository jobRepository,
    ICurrentUser currentUser)
    : ICommandHandler<ConfirmServiceCompletionCommand, Result<Success>>
{
    public async Task<Result<Success>> Handle(
        ConfirmServiceCompletionCommand command,
        CancellationToken cancellationToken)
    {
        // 1. Resolve the authenticated user's customer profile. This is also
        //    the customer-only authorization gate: only a customer can confirm
        //    service completion, so a user without a customer profile (for
        //    example a technician) is rejected.
        var customerProfile = await customerRepository.GetByUserIdAsync(
            currentUser.UserId,
            cancellationToken);

        if (customerProfile is null)
        {
            return CustomerProfileErrors.NotFound;
        }

        // 2. Load the job.
        var job = await jobRepository.GetByIdAsync(
            command.JobId,
            cancellationToken);

        if (job is null)
        {
            return JobErrors.NotFound;
        }

        // 3. Verify the job belongs to the authenticated customer. Ownership
        //    is derived from the authenticated identity through CustomerProfile
        //    -> ServiceRequest -> Job using a lightweight projection, so the
        //    ServiceRequest graph is never loaded. An un-owned job is
        //    indistinguishable from a non-existent one, so job existence is
        //    never leaked.
        var jobAccess = await jobRepository.GetAccessAsync(
            command.JobId,
            cancellationToken);

        if (jobAccess is null ||
            jobAccess.ServiceRequestCustomerProfileId != customerProfile.Id)
        {
            return JobErrors.NotFound;
        }

        // 4. Apply the domain transition. The aggregate owns the rules: only a
        //    completed job can be confirmed, and confirmation can happen only
        //    once. The Job remains Completed; confirmation is a business state
        //    associated with completion.
        var confirmationResult = job.ConfirmCompletion();

        if (confirmationResult.IsError)
        {
            return confirmationResult.Errors;
        }

        // 5. Persist the confirmation, its timestamp, and the timeline event
        //    together (committed atomically by the transaction pipeline).
        jobRepository.Update(job);

        return Result.Success;
    }
}
