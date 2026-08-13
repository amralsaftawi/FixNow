using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;

namespace FixNow.Application.Features.Assignments.Commands.ReassignServiceRequest;

public sealed class ReassignServiceRequestCommandHandler(
    ICustomerRepository customerRepository,
    IServiceRequestRepository serviceRequestRepository,
    ITechnicianProfileRepository technicianProfileRepository,
    IAssignmentRepository assignmentRepository,
    ICurrentUser currentUser)
    : ICommandHandler<ReassignServiceRequestCommand, Result<Success>>
{
    public async Task<Result<Success>> Handle(
        ReassignServiceRequestCommand command,
        CancellationToken cancellationToken)
    {
        // 1. Resolve the authenticated user's customer profile. Only the
        //    request owner may reassign it; the identity is derived from the
        //    current user, never from the client.
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

        // 4. A request can only be reassigned while it is assigned to a
        //    technician who has not yet responded (Assigned state). Once the
        //    technician has accepted or the request has moved to another
        //    state, the technician can no longer be replaced.
        if (serviceRequest.Status != ServiceRequestStatus.Assigned)
        {
            return ServiceRequestErrors.InvalidStatusTransition;
        }

        // 5. Load the new technician with their services.
        var technicianProfile = await technicianProfileRepository.GetByIdWithServicesAsync(
            command.TechnicianProfileId,
            cancellationToken);

        if (technicianProfile is null)
        {
            return TechnicianProfileErrors.NotFound;
        }

        // 6. The new technician must provide the requested service category.
        //    This is the same eligibility rule used by the assign and accept
        //    flows.
        var providesCategory = technicianProfile.Services
            .Any(service => service.ServiceCategoryId == serviceRequest.ServiceCategoryId);

        if (!providesCategory)
        {
            return TechnicianProfileErrors.ServiceCategoryNotProvided;
        }

        // 7. Load the current pending assignment being replaced.
        var currentAssignment = await assignmentRepository.GetPendingByRequestAsync(
            serviceRequest.Id,
            cancellationToken);

        if (currentAssignment is null)
        {
            return AssignmentErrors.NotAssigned;
        }

        // 8. Reassigning to the currently assigned technician is a no-op.
        if (currentAssignment.TechnicianProfileId == technicianProfile.Id)
        {
            return AssignmentErrors.SameTechnician;
        }

        // 9. Record the reassignment on the request (timeline + domain
        //    event). The request stays in the Assigned state.
        var reassignResult = serviceRequest.Reassign();

        if (reassignResult.IsError)
        {
            return reassignResult.Errors;
        }

        // 10. Cancel the old assignment - its row is kept as audit history -
        //     then create the new pending assignment for the new technician.
        var cancelResult = currentAssignment.Cancel();

        if (cancelResult.IsError)
        {
            return cancelResult.Errors;
        }

        var assignmentResult = Assignment.Create(
            Guid.NewGuid(),
            serviceRequest.Id,
            technicianProfile.Id);

        if (assignmentResult.IsError)
        {
            return assignmentResult.Errors;
        }

        // 11. Persist both changes (committed by the transaction pipeline).
        //     Optimistic concurrency (PostgreSQL xmin row version) serializes
        //     concurrent reassignments: the losing request conflicts on the
        //     ServiceRequest row version and is rejected with 409.
        serviceRequestRepository.Update(serviceRequest);

        await assignmentRepository.AddAsync(
            assignmentResult.Value,
            cancellationToken);

        return Result.Success;
    }
}
