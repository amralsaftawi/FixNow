using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;

namespace FixNow.Application.Features.Jobs.Commands.UpdateTechnicianLocation;

public sealed class UpdateTechnicianLocationCommandHandler(
    ITechnicianProfileRepository technicianProfileRepository,
    IJobRepository jobRepository,
    IAssignmentRepository assignmentRepository,
    ICurrentUser currentUser)
    : ICommandHandler<UpdateTechnicianLocationCommand, Result<Success>>
{
    public async Task<Result<Success>> Handle(
        UpdateTechnicianLocationCommand command,
        CancellationToken cancellationToken)
    {
        // 1. Resolve the authenticated user's technician profile. This is
        //    also the technician-only authorization gate: a user without a
        //    technician profile cannot publish a location.
        var technicianProfile = await technicianProfileRepository.GetByUserIdAsync(
            currentUser.UserId,
            cancellationToken);

        if (technicianProfile is null)
        {
            return TechnicianProfileErrors.NotFound;
        }

        // 2. Load the job the technician is publishing a location for.
        var job = await jobRepository.GetByIdAsync(
            command.JobId,
            cancellationToken);

        if (job is null)
        {
            return JobErrors.NotFound;
        }

        // 3. Only the technician assigned to the job may publish a location
        //    for it. An un-owned job is indistinguishable from a non-existent
        //    one, so job existence is never leaked.
        if (job.TechnicianProfileId != technicianProfile.Id)
        {
            return JobErrors.NotFound;
        }

        // 4. The assignment must still be active - accepted, not rejected,
        //    cancelled, or otherwise inactive - for the technician on the
        //    job's service request. An inactive assignment grants no access,
        //    and the outcome is indistinguishable from a non-existent job.
        var assignment = await assignmentRepository.GetAcceptedByRequestAndTechnicianAsync(
            job.ServiceRequestId,
            technicianProfile.Id,
            cancellationToken);

        if (assignment is null)
        {
            return JobErrors.NotFound;
        }

        // 5. A terminal job (completed or cancelled) no longer accepts
        //    location updates. The state machine treats these states as
        //    terminal, so any location published for them is rejected.
        if (job.IsTerminated)
        {
            return JobErrors.InvalidStatusTransition;
        }

        // 6. Record the latest current location on the technician profile,
        //    updated in place (single record, no history).
        var updateResult = technicianProfile.UpdateCurrentLocation(
            command.Latitude,
            command.Longitude);

        if (updateResult.IsError)
        {
            return updateResult.Errors;
        }

        // 7. Persist the change (committed by the transaction pipeline).
        technicianProfileRepository.Update(technicianProfile);

        return Result.Success;
    }
}
