public sealed class ServiceRequestTimeline : AuditableEntity
{
    public Guid ServiceRequestId { get; private set; }

    public ServiceRequestStatus Status { get; private set; }

    public string Description { get; private set; }

    public DateTimeOffset OccurredOn { get; private set; }

    // Navigation

    public ServiceRequest ServiceRequest { get; private set; } = null!;

    private ServiceRequestTimeline()
    {
    }

    private ServiceRequestTimeline(
        Guid id,
        Guid serviceRequestId,
        ServiceRequestStatus status,
        string description)
        : base(id)
    {
        ServiceRequestId = serviceRequestId;

        Status = status;

        Description = description;

        OccurredOn = DateTimeOffset.UtcNow;
    }

    public static Result<ServiceRequestTimeline> Create(
        Guid id,
        Guid serviceRequestId,
        ServiceRequestStatus status,
        string description)
    {
        if (id == Guid.Empty)
            return ServiceRequestTimelineErrors.IdRequired;

        if (serviceRequestId == Guid.Empty)
            return ServiceRequestTimelineErrors.ServiceRequestIdRequired;

        if (string.IsNullOrWhiteSpace(description))
            return ServiceRequestTimelineErrors.DescriptionRequired;

        description = description.Trim();

        if (description.Length > 500)
            return ServiceRequestTimelineErrors.DescriptionTooLong;

        return new ServiceRequestTimeline(
            id,
            serviceRequestId,
            status,
            description);
    }
}