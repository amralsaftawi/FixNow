using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;

namespace FixNow.Application.Features.Jobs.Queries.GetFinalJobPrice;

public sealed class GetFinalJobPriceQueryHandler(
    ICustomerRepository customerRepository,
    ITechnicianProfileRepository technicianProfileRepository,
    IJobRepository jobRepository,
    ICurrentUser currentUser)
    : IQueryHandler<GetFinalJobPriceQuery, Result<GetFinalJobPriceResponse>>
{
    public async Task<Result<GetFinalJobPriceResponse>> Handle(
        GetFinalJobPriceQuery query,
        CancellationToken cancellationToken)
    {
        // 1. Resolve the authenticated user's profiles. The final price is
        //    accessible to the customer who owns the job's service request
        //    and to the technician the job is assigned to. A user holding
        //    neither profile can never be authorized for a job.
        var customerProfile = await customerRepository.GetByUserIdAsync(
            currentUser.UserId,
            cancellationToken);

        var technicianProfile = await technicianProfileRepository.GetByUserIdAsync(
            currentUser.UserId,
            cancellationToken);

        // 2. Load the job's ownership facts as a lightweight projection.
        //    The full Job aggregate (and its ServiceRequest graph) is never
        //    loaded for authorization.
        var jobAccess = await jobRepository.GetAccessAsync(
            query.JobId,
            cancellationToken);

        if (jobAccess is null)
        {
            return JobErrors.NotFound;
        }

        // 3. Authorize. Customer access is derived from the authenticated
        //    identity through CustomerProfile -> ServiceRequest -> Job, and
        //    technician access from the technician assigned to the job. An
        //    out-of-scope job is indistinguishable from a non-existent one,
        //    so job existence is never leaked through JobId manipulation.
        var isCustomerOwner =
            customerProfile is not null
            && customerProfile.Id == jobAccess.ServiceRequestCustomerProfileId;

        var isAssignedTechnician =
            technicianProfile is not null
            && technicianProfile.Id == jobAccess.TechnicianProfileId;

        if (!isCustomerOwner && !isAssignedTechnician)
        {
            return JobErrors.NotFound;
        }

        // 4. Load the persisted pricing components as a focused projection.
        var pricing = await jobRepository.GetFinalJobPriceAsync(
            query.JobId,
            cancellationToken);

        if (pricing is null)
        {
            return JobErrors.NotFound;
        }

        // 5. Resolve the service price component. For a finalized (completed)
        //    job the snapshot captured at completion is authoritative, so
        //    later technician/category price changes can never alter it. For
        //    an active job the technician's configured price for the job's
        //    category is preferred, falling back to the category base price.
        //    This mirrors the established base-price resolution.
        var servicePrice = pricing.FinalizedServicePrice;

        if (servicePrice is null)
        {
            var technicianPrice =
                await technicianProfileRepository.GetServicePriceByCategoryAsync(
                    jobAccess.TechnicianProfileId,
                    pricing.ServiceCategoryId,
                    cancellationToken);

            servicePrice = technicianPrice ?? pricing.BasePrice;
        }

        // 6. Resolve the inspection fee component from the finalized snapshot
        //    when present, otherwise from the category configuration.
        var inspectionFee = pricing.FinalizedInspectionFee ?? pricing.InspectionFee;

        // 7. Resolve the additional charges component as the sum of the job's
        //    recorded charges. The individual charge records are never
        //    modified; charges are immutable and cannot be added to a
        //    terminated job, so the total is stable once finalized.
        Money? additionalChargesTotal = null;

        if (pricing.AdditionalChargesTotal > 0
            && pricing.AdditionalChargesCurrency is { } chargeCurrency)
        {
            var chargeAmount = Money.Create(
                pricing.AdditionalChargesTotal,
                chargeCurrency);

            if (chargeAmount.IsSuccess)
            {
                additionalChargesTotal = chargeAmount.Value;
            }
        }

        // 8. Aggregate the components into the final price using the domain
        //    monetary rules. No client-supplied value ever participates.
        var finalPrice = Money.Sum(
            servicePrice,
            inspectionFee,
            additionalChargesTotal);

        return new GetFinalJobPriceResponse(
            JobId: query.JobId,
            Status: pricing.Status,
            IsFinalized: pricing.Status == JobStatus.Completed,
            ServicePrice: servicePrice,
            InspectionFee: inspectionFee,
            AdditionalChargesTotal: additionalChargesTotal,
            FinalPrice: finalPrice);
    }
}
