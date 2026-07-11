public sealed class ServiceRequestTimeline : AuditableEntity
{
    public Guid ServiceRequestId { get; private set; }

    public ServiceRequestStatus Status { get; private set; }

    public string Message { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }
#pragma warning disable CS8618
    private ServiceRequestTimeline()
    {
    }
#pragma warning disable CS8618
    private ServiceRequestTimeline(
        Guid id,
        Guid serviceRequestId,
        ServiceRequestStatus status,
        string message)
        : base(id)
    {
        ServiceRequestId = serviceRequestId;
        Status = status;
        Message = message;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public static Result<ServiceRequestTimeline> Create(
        Guid id,
        Guid serviceRequestId,
        ServiceRequestStatus status,
        string message)
    {
       
        return new ServiceRequestTimeline(
            id,
            serviceRequestId,
            status,
            message);
    }
}
