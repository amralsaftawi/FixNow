namespace FixNow.Application.Features.Reviews.Commands.CreateReview;

public sealed record CreateReviewResponse(
    Guid ReviewId,
    Guid JobId,
    Guid TechnicianId,
    string Comment,
    DateTimeOffset CreatedAtUtc);
