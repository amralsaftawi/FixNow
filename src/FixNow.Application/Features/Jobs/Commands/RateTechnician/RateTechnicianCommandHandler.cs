using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;

namespace FixNow.Application.Features.Jobs.Commands.RateTechnician;

public sealed class RateTechnicianCommandHandler(
    ICustomerRepository customerRepository,
    IJobRepository jobRepository,
    IAssignmentRepository assignmentRepository,
    IReviewRepository reviewRepository,
    ICurrentUser currentUser)
    : ICommandHandler<RateTechnicianCommand, Result<RateTechnicianResponse>>
{
    public async Task<Result<RateTechnicianResponse>> Handle(
        RateTechnicianCommand command,
        CancellationToken cancellationToken)
    {
        // 1. Resolve the authenticated user's customer profile. Only
        //    customers can rate technicians.
        var customerProfile = await customerRepository.GetByUserIdAsync(
            currentUser.UserId,
            cancellationToken);

        if (customerProfile is null)
        {
            return CustomerProfileErrors.NotFound;
        }

        // 2. Load the job by id.
        var job = await jobRepository.GetByIdAsync(
            command.JobId,
            cancellationToken);

        if (job is null)
        {
            return JobErrors.NotFound;
        }

        // 3. Verify the job belongs to the authenticated customer using the
        //    lightweight ownership projection.
        var jobAccess = await jobRepository.GetAccessAsync(
            command.JobId,
            cancellationToken);

        if (jobAccess is null ||
            jobAccess.ServiceRequestCustomerProfileId != customerProfile.Id)
        {
            return JobErrors.NotFound;
        }

        // 4. Only completed jobs can be rated.
        if (job.Status != JobStatus.Completed)
        {
            return ReviewErrors.JobNotCompleted;
        }

        // 5. Find the assignment that links this technician to the service
        //    request. The assignment is required to create a review because
        //    the unique constraint prevents duplicate reviews per assignment.
        var assignment = await assignmentRepository.GetByRequestAndTechnicianAsync(
            job.ServiceRequestId,
            job.TechnicianProfileId,
            cancellationToken);

        if (assignment is null)
        {
            return AssignmentErrors.NotFound;
        }

        // 6. Check if a review already exists for this assignment.
        var alreadyRated = await reviewRepository.ExistsByAssignmentIdAsync(
            assignment.Id,
            cancellationToken);

        if (alreadyRated)
        {
            return ReviewErrors.AlreadyRated;
        }

        // 7. Create the Rating value object.
        var ratingResult = Rating.Create(command.Rating);

        if (ratingResult.IsError)
        {
            return ratingResult.Errors;
        }

        // 8. Create the Review aggregate through its domain factory.
        var reviewResult = Review.Create(
            id: Guid.NewGuid(),
            assignmentId: assignment.Id,
            serviceRequestId: job.ServiceRequestId,
            customerProfileId: customerProfile.Id,
            technicianProfileId: job.TechnicianProfileId,
            rating: ratingResult.Value,
            comment: command.Comment);

        if (reviewResult.IsError)
        {
            return reviewResult.Errors;
        }

        // 9. Persist the new review.
        await reviewRepository.AddAsync(
            reviewResult.Value,
            cancellationToken);

        return new RateTechnicianResponse(
            ReviewId: reviewResult.Value.Id,
            Rating: command.Rating);
    }
}
