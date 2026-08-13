using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;

namespace FixNow.Application.Features.TechnicianRequests.Commands.RejectServiceRequest;

public sealed class RejectServiceRequestCommandHandler(
    ITechnicianProfileRepository technicianProfileRepository,
    IServiceRequestRepository serviceRequestRepository,
    IAssignmentRepository assignmentRepository,
    ICurrentUser currentUser)
    : ICommandHandler<RejectServiceRequestCommand, Result<Success>>
{
    public async Task<Result<Success>> Handle(
        RejectServiceRequestCommand command,
        CancellationToken cancellationToken)
    {
        // 1. Resolve the authenticated user's technician profile. This is
        //    also the technician-only authorization gate: a user without a
        //    technician profile cannot reject service requests.
        var technicianProfile = await technicianProfileRepository.GetByUserIdWithServicesAsync(
            currentUser.UserId,
            cancellationToken);

        if (technicianProfile is null)
        {
            return TechnicianProfileErrors.NotFound;
        }

        // 2. Load the service request.
        var serviceRequest = await serviceRequestRepository.GetByIdAsync(
            command.ServiceRequestId,
            cancellationToken);

        if (serviceRequest is null)
        {
            return ServiceRequestErrors.NotFound;
        }

        // 3. The technician may only act on requests in one of their service
        //    categories. An out-of-scope request is indistinguishable from a
        //    non-existent one, so request existence is never leaked.
        var isInScope = technicianProfile.Services
            .Any(service => service.ServiceCategoryId == serviceRequest.ServiceCategoryId);

        if (!isInScope)
        {
            return ServiceRequestErrors.NotFound;
        }

        // 4. Only a request that is still being offered to technicians can
        //    be rejected. Accepted, in-progress, completed, or cancelled
        //    requests are not rejectable.
        if (serviceRequest.Status != ServiceRequestStatus.SearchingTechnician)
        {
            return ServiceRequestErrors.InvalidStatusTransition;
        }

        // 5. Record the rejection through the existing Assignment domain
        //    model (mirrors Accept: create the assignment, then apply the
        //    transition). The ServiceRequest itself is intentionally left in
        //    SearchingTechnician state so it remains available to other
        //    eligible technicians - this is not an unassign/reassign.
        var assignmentResult = Assignment.Create(
            Guid.NewGuid(),
            serviceRequest.Id,
            technicianProfile.Id);

        if (assignmentResult.IsError)
        {
            return assignmentResult.Errors;
        }

        var assignment = assignmentResult.Value;

        var rejectResult = assignment.Reject(command.Reason);

        if (rejectResult.IsError)
        {
            return rejectResult.Errors;
        }

        // 6. Persist the change (committed by the transaction pipeline).
        await assignmentRepository.AddAsync(
            assignment,
            cancellationToken);

        return Result.Success;
    }
}
