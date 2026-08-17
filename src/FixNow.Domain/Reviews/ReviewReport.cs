public sealed class ReviewReport : AuditableEntity
{
    public Guid ReviewId { get; private set; }

    public Guid ReporterUserId { get; private set; }

    public ReviewReportReason Reason { get; private set; }

    public string? Description { get; private set; }

    public ReviewReportStatus Status { get; private set; }

    // Navigation

    public Review Review { get; private set; } = null!;

    public User ReporterUser { get; private set; } = null!;

#pragma warning disable CS8618
    private ReviewReport()
    {
    }
#pragma warning disable CS8618
    private ReviewReport(
        Guid id,
        Guid reviewId,
        Guid reporterUserId,
        ReviewReportReason reason,
        string? description)
        : base(id)
    {
        ReviewId = reviewId;
        ReporterUserId = reporterUserId;
        Reason = reason;
        Description = description;
        Status = ReviewReportStatus.Pending;
    }

    public static Result<ReviewReport> Create(
        Guid id,
        Guid reviewId,
        Guid reporterUserId,
        ReviewReportReason reason,
        string? description = null)
    {
        if (id == Guid.Empty)
            return ReviewReportErrors.IdRequired;

        if (reviewId == Guid.Empty)
            return ReviewReportErrors.ReviewIdRequired;

        if (reporterUserId == Guid.Empty)
            return ReviewReportErrors.ReporterUserIdRequired;

        description = description?.Trim();

        if (description?.Length > 1000)
            return ReviewReportErrors.DescriptionTooLong;

        var report = new ReviewReport(
            id,
            reviewId,
            reporterUserId,
            reason,
            description);

        report.AddDomainEvent(
            new ReviewReportedDomainEvent(
                report.Id,
                report.ReviewId,
                report.ReporterUserId));

        return report;
    }
}
