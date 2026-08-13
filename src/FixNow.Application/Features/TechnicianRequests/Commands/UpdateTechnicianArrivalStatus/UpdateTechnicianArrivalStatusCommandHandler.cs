using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;

namespace FixNow.Application.Features.TechnicianRequests.Commands.UpdateTechnicianArrivalStatus;

public sealed class UpdateTechnicianArrivalStatusCommandHandler(
    ITechnicianProfileRepository technicianProfileRepository,
    IServiceRequestRepository serviceRequestRepository,
    IAssignmentRepository assignmentRepository,
    ICurrentUser currentUser)
    : ICommandHandler<UpdateTechnicianArrivalStatusCommand, Result<Success>>
{
    public async Task<Result<Success>> Handle(
        UpdateTechnicianArrivalStatusCommand command,
        CancellationToken cancellationToken)
    {
        // 1. Resolve the authenticated user's technician profile. This is
        //    also the technician-only authorization gate: a user without a
        //    technician profile cannot update arrival status.
        var technicianProfile = await technicianProfileRepository.GetByUserIdAsync(
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

        // 3. Only the technician holding an accepted assignment for the
        //    request may update its arrival status. A rejected, cancelled, or
        //    otherwise inactive assignment grants no access, and an
        //    out-of-scope request is indistinguishable from a non-existent
        //    one, so request existence is never leaked.
        var assignment = await assignmentRepository.GetAcceptedByRequestAndTechnicianAsync(
            serviceRequest.Id,
            technicianProfile.Id,
            cancellationToken);

        if (assignment is null)
        {
            return ServiceRequestErrors.NotFound;
        }

        // 4. Apply the matching domain transition. The strict
        //    predecessor-only rules on the aggregate guarantee the correct
        //    Accepted -> OnTheWay -> Arrived -> InProgress order.
        var transitionResult = command.Status switch
        {
            TechnicianArrivalStatus.OnTheWay => serviceRequest.MarkOnTheWay(),
            TechnicianArrivalStatus.Arrived => serviceRequest.MarkArrived(),
            TechnicianArrivalStatus.VisitStarted => serviceRequest.MarkInProgress(),
            _ => ServiceRequestErrors.InvalidStatusTransition
        };

        if (transitionResult.IsError)
        {
            return transitionResult.Errors;
        }

        // 5. Persist the change (committed by the transaction pipeline).
        serviceRequestRepository.Update(serviceRequest);

        return Result.Success;
    }
}
