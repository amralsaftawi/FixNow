using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Application.Common.Interfaces.Services;

namespace FixNow.Application.Features.Jobs.Queries.GetCustomerJobEta;

public sealed class GetCustomerJobEtaQueryHandler(
    ICustomerRepository customerRepository,
    IServiceRequestRepository serviceRequestRepository,
    IJobRepository jobRepository,
    ITechnicianProfileRepository technicianProfileRepository,
    IEstimatedArrivalTimeService estimatedArrivalTimeService,
    ICurrentUser currentUser)
    : IQueryHandler<GetCustomerJobEtaQuery, Result<GetCustomerJobEtaResponse>>
{
    public async Task<Result<GetCustomerJobEtaResponse>> Handle(
        GetCustomerJobEtaQuery query,
        CancellationToken cancellationToken)
    {
        // 1. Resolve the authenticated user's customer profile. This is
        //    also the customer-only authorization gate: a user without a
        //    customer profile cannot request ETA.
        var customerProfile = await customerRepository.GetByUserIdAsync(
            currentUser.UserId,
            cancellationToken);

        if (customerProfile is null)
        {
            return CustomerProfileErrors.NotFound;
        }

        // 2. Load the job.
        var job = await jobRepository.GetByIdAsync(
            query.JobId,
            cancellationToken);

        if (job is null)
        {
            return JobErrors.NotFound;
        }

        // 3. Verify ownership and load the destination coordinates. The
        //    ownership check is applied inside the database query, so a job
        //    belonging to another customer is indistinguishable from a
        //    non-existent one. This also guarantees the customer can never
        //    supply or override the Job destination.
        var destination = await serviceRequestRepository.GetDestinationForCustomerAsync(
            job.ServiceRequestId,
            customerProfile.Id,
            cancellationToken);

        if (destination is null)
        {
            return JobErrors.NotFound;
        }

        // 4. ETA only represents a future arrival while the technician is
        //    actively traveling. A job that has not been dispatched yet, has
        //    already arrived, is in progress, or is terminal (completed or
        //    cancelled) does not produce a meaningful arrival estimate. The
        //    final Job lifecycle state remains observable.
        if (job.Status != JobStatus.OnTheWay)
        {
            return Unavailable(job.Id, job.Status);
        }

        // 5. Read the latest technician location produced by the existing
        //    Real-Time Technician Location mechanism. A location that has
        //    never been published resolves to an unavailable estimate.
        var technicianLocation = await technicianProfileRepository.GetLocationAsync(
            job.TechnicianProfileId,
            cancellationToken);

        if (technicianLocation is null ||
            technicianLocation.Latitude is null ||
            technicianLocation.Longitude is null)
        {
            return Unavailable(job.Id, job.Status);
        }

        // 6. Calculate the ETA through the centralized estimation service.
        //    The reference time is supplied in UTC so the estimate is
        //    deterministic and time-zone independent.
        var estimate = estimatedArrivalTimeService.Estimate(
            technicianLocation.Latitude.Value,
            technicianLocation.Longitude.Value,
            destination.Latitude,
            destination.Longitude,
            DateTimeOffset.UtcNow);

        if (estimate is null)
        {
            return Unavailable(job.Id, job.Status);
        }

        return new GetCustomerJobEtaResponse(
            JobId: job.Id,
            Status: job.Status,
            IsEstimateAvailable: true,
            EstimatedArrivalTimeUtc: estimate.EstimatedArrivalTimeUtc,
            EstimatedTravelMinutes: estimate.EstimatedTravelMinutes,
            DistanceKm: estimate.DistanceKm);
    }

    private static GetCustomerJobEtaResponse Unavailable(
        Guid jobId,
        JobStatus status)
        => new(
            JobId: jobId,
            Status: status,
            IsEstimateAvailable: false,
            EstimatedArrivalTimeUtc: null,
            EstimatedTravelMinutes: null,
            DistanceKm: null);
}
