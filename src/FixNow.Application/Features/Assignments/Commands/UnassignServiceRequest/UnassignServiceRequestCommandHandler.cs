using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;

namespace FixNow.Application.Features.Assignments.Commands.UnassignServiceRequest;

public sealed class UnassignServiceRequestCommandHandler(
    ICustomerRepository customerRepository,
    IServiceRequestRepository serviceRequestRepository,
    IAssignmentRepository assignmentRepository,
    ICurrentUser currentUser)
    : ICommandHandler<UnassignServiceRequestCommand, Result<Success>>
{
    public async Task<Result<Success>> Handle(
        UnassignServiceRequestCommand command,
        CancellationToken cancellationToken)
    {
        // 1. Resolve the authenticated user's customer profile. Only the
        //    request owner may remove the technician assignment; the identity
        //    is derived from the current user, never from the client.
        var customerProfile = await customerRepository.GetByUserIdAsync(
            currentUser.UserId,
            cancellationToken);

        if (customerProfile is null)
        {
            return CustomerProfileErrors.NotFound;
        }

        // 2. Load the service request.
        var serviceRequest = await serviceRequestRepository.GetByIdAsync(
            command.ServiceRequestId,
            cancellationToken);

        if (serviceRequest is null)
        {
            return ServiceRequestErrors.NotFound;
        }

        // 3. Verify the request belongs to the current customer. An un-owned
        //    request is indistinguishable from a non-existent one, so request
        //    existence is never leaked.
        if (serviceRequest.CustomerProfileId != customerProfile.Id)
        {
            return ServiceRequestErrors.NotFound;
        }

        // 4. A technician assignment can only be removed while the request is
        //    assigned to a technician who has not yet responded (Assigned
        //    state). Accepted, in-progress, completed, or cancelled requests
        //    are not affected by this operation.
        if (serviceRequest.Status != ServiceRequestStatus.Assigned)
        {
            return ServiceRequestErrors.InvalidStatusTransition;
        }

        // 5. Load the current pending assignment being removed.
        var currentAssignment = await assignmentRepository.GetPendingByRequestAsync(
            serviceRequest.Id,
            cancellationToken);

        if (currentAssignment is null)
        {
            return AssignmentErrors.NotAssigned;
        }

        // 6. Cancel the assignment - its row is kept as audit history - and
        //    return the request to the SearchingTechnician state so it is
        //    available to other eligible technicians again.
        var cancelResult = currentAssignment.Cancel();

        if (cancelResult.IsError)
        {
            return cancelResult.Errors;
        }

        var unassignResult = serviceRequest.Unassign();

        if (unassignResult.IsError)
        {
            return unassignResult.Errors;
        }

        // 7. Persist both changes (committed by the transaction pipeline).
        //    Optimistic concurrency (PostgreSQL xmin row version) serializes
        //    concurrent unassign/reassign/accept operations on the same
        //    request: the losing request conflicts and is rejected with 409.
        serviceRequestRepository.Update(serviceRequest);

        return Result.Success;
    }
}
