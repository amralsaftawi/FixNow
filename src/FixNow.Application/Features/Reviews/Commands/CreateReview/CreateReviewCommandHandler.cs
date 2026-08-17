using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;

namespace FixNow.Application.Features.Reviews.Commands.CreateReview;

public sealed class CreateReviewCommandHandler(
    ICustomerRepository customerRepository,
    IJobRepository jobRepository,
    IAssignmentRepository assignmentRepository,
    IReviewRepository reviewRepository,
    ICurrentUser currentUser)
    : ICommandHandler<CreateReviewCommand, Result<CreateReviewResponse>>
{
    public async Task<Result<CreateReviewResponse>> Handle(
        CreateReviewCommand command,
        CancellationToken cancellationToken)
    {
        // 1. Resolve the authenticated user's customer profile. Only
        //    customers can submit written reviews.
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
        //    lightweight ownership projection. An un-owned job is
        //    indistinguishable from a non-existent one.
        var jobAccess = await jobRepository.GetAccessAsync(
            command.JobId,
            cancellationToken);

        if (jobAccess is null ||
            jobAccess.ServiceRequestCustomerProfileId != customerProfile.Id)
        {
            return JobErrors.NotFound;
        }

        // 4. Only completed jobs can receive a written review.
        if (job.Status != JobStatus.Completed)
        {
            return ReviewErrors.JobNotCompleted;
        }

        // 5. Find the assignment that links this technician to the service
        //    request. The assignment is required because the unique
        //    constraint on AssignmentId prevents duplicate reviews per job.
        var assignment = await assignmentRepository.GetByRequestAndTechnicianAsync(
            job.ServiceRequestId,
            job.TechnicianProfileId,
            cancellationToken);

        if (assignment is null)
        {
            return AssignmentErrors.NotFound;
        }

        // 6. Check if a review already exists for this assignment. The
        //    unique database constraint prevents duplicate reviews, but
        //    the application-level check provides a clear business error.
        var alreadyReviewed = await reviewRepository.ExistsByAssignmentIdAsync(
            assignment.Id,
            cancellationToken);

        if (alreadyReviewed)
        {
            return ReviewErrors.AlreadyRated;
        }

        // 7. Trim the comment and validate it is not empty after trimming.
        //    The FluentValidation validator handles this at the boundary,
        //    but the handler enforces the invariant for safety.
        var trimmedComment = command.Comment.Trim();

        if (string.IsNullOrWhiteSpace(trimmedComment))
        {
            return ReviewErrors.CommentEmpty;
        }

        // 8. Create the Rating value object with 0 (unrated). This is a
        //    written review — the customer provides text feedback, not a
        //    numeric rating. Rating = 0 distinguishes this from a rating-
        //    only submission created by the RateTechnician feature.
        var ratingResult = Rating.Create(0);

        if (ratingResult.IsError)
        {
            return ratingResult.Errors;
        }

        // 9. Create the Review aggregate through its domain factory.
        var reviewResult = Review.Create(
            id: Guid.NewGuid(),
            assignmentId: assignment.Id,
            serviceRequestId: job.ServiceRequestId,
            customerProfileId: customerProfile.Id,
            technicianProfileId: job.TechnicianProfileId,
            rating: ratingResult.Value,
            comment: trimmedComment);

        if (reviewResult.IsError)
        {
            return reviewResult.Errors;
        }

        // 10. Persist the new review. The Unit of Work pipeline commits
        //     the change atomically.
        await reviewRepository.AddAsync(
            reviewResult.Value,
            cancellationToken);

        return new CreateReviewResponse(
            ReviewId: reviewResult.Value.Id,
            JobId: command.JobId,
            TechnicianId: job.TechnicianProfileId,
            Comment: trimmedComment,
            CreatedAtUtc: reviewResult.Value.CreatedAtUtc);
    }
}
