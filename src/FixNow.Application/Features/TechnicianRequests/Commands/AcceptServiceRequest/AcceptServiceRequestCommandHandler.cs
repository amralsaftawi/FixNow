using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;

namespace FixNow.Application.Features.TechnicianRequests.Commands.AcceptServiceRequest;

public sealed class AcceptServiceRequestCommandHandler(
    ITechnicianProfileRepository technicianProfileRepository,
    IServiceRequestRepository serviceRequestRepository,
    IAssignmentRepository assignmentRepository,
    ICurrentUser currentUser)
    : ICommandHandler<AcceptServiceRequestCommand, Result<Success>>
{
    public async Task<Result<Success>> Handle(
        AcceptServiceRequestCommand command,
        CancellationToken cancellationToken)
    {
        // 1. Resolve the authenticated user's technician profile. This is
        //    also the technician-only authorization gate: a user without a
        //    technician profile cannot accept service requests.
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

        // 3. The technician may only accept requests in one of their service
        //    categories. An out-of-scope request is indistinguishable from a
        //    non-existent one, so request existence is never leaked.
        var isInScope = technicianProfile.Services
            .Any(service => service.ServiceCategoryId == serviceRequest.ServiceCategoryId);

        if (!isInScope)
        {
            return ServiceRequestErrors.NotFound;
        }

        // 4. Apply the transition through the domain model. A request that is
        //    still searching for a technician can be accepted directly; a
        //    request that was explicitly assigned to a technician (Assigned)
        //    can only be accepted by the assigned technician. Other states
        //    (accepted, in progress, completed, cancelled) are rejected here.
        global::Assignment assignment;
        var isNewAssignment = false;

        if (serviceRequest.Status == ServiceRequestStatus.Assigned)
        {
            // The accepting technician must be the technician the request was
            // assigned to. A non-assignee is indistinguishable from a
            // non-existent assignment, so assignment existence is never
            // leaked.
            var pendingAssignment = await assignmentRepository
                .GetPendingByRequestAndTechnicianAsync(
                    serviceRequest.Id,
                    technicianProfile.Id,
                    cancellationToken);

            if (pendingAssignment is null)
            {
                return ServiceRequestErrors.NotFound;
            }

            assignment = pendingAssignment;
        }
        else
        {
            // Accepting a searching request establishes the technician as the
            // assigned technician using the existing Assignment domain model.
            var assignmentResult = Assignment.Create(
                Guid.NewGuid(),
                serviceRequest.Id,
                technicianProfile.Id);

            if (assignmentResult.IsError)
            {
                return assignmentResult.Errors;
            }

            assignment = assignmentResult.Value;
            isNewAssignment = true;
        }

        var transitionResult = serviceRequest.Accept();

        if (transitionResult.IsError)
        {
            return transitionResult.Errors;
        }

        var assignmentAcceptResult = assignment.Accept();

        if (assignmentAcceptResult.IsError)
        {
            return assignmentAcceptResult.Errors;
        }

        // 5. Persist both changes (committed by the transaction pipeline).
        //    Optimistic concurrency (PostgreSQL xmin row version) ensures a
        //    concurrent acceptance cannot be applied twice: the second
        //    request conflicts on the ServiceRequest row version.
        serviceRequestRepository.Update(serviceRequest);

        if (isNewAssignment)
        {
            await assignmentRepository.AddAsync(
                assignment,
                cancellationToken);
        }

        return Result.Success;
    }
}
