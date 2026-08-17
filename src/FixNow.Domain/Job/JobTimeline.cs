public sealed class JobTimeline : AuditableEntity
{
    public Guid JobId { get; private set; }

    public JobStatus Status { get; private set; }

    public string Description { get; private set; }

    public DateTimeOffset OccurredOn { get; private set; }

    // Navigation

    public Job Job { get; private set; } = null!;

    private JobTimeline()
    {
    }

    private JobTimeline(
        Guid id,
        Guid jobId,
        JobStatus status,
        string description)
        : base(id)
    {
        JobId = jobId;

        Status = status;

        Description = description;

        OccurredOn = DateTimeOffset.UtcNow;
    }

    public static Result<JobTimeline> Create(
        Guid id,
        Guid jobId,
        JobStatus status,
        string description)
    {
        if (id == Guid.Empty)
            return JobTimelineErrors.IdRequired;

        if (jobId == Guid.Empty)
            return JobTimelineErrors.JobIdRequired;

        if (string.IsNullOrWhiteSpace(description))
            return JobTimelineErrors.DescriptionRequired;

        description = description.Trim();

        if (description.Length > 500)
            return JobTimelineErrors.DescriptionTooLong;

        return new JobTimeline(
            id,
            jobId,
            status,
            description);
    }
}
