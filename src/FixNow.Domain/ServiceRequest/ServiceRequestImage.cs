public sealed class ServiceRequestImage : AuditableEntity
{
     public Guid ServiceRequestId { get; private set; }

    public string ImageKey { get; private set; }

    public DateTimeOffset UploadedAt { get; private set; }

#pragma warning disable CS8618
    private ServiceRequestImage()
    {
    }
#pragma warning disable CS8618
    private ServiceRequestImage(
        Guid id,
        Guid serviceRequestId,
        string imageKey)
        : base(id)
    {
        ServiceRequestId = serviceRequestId;
        ImageKey = imageKey;
        UploadedAt = DateTimeOffset.UtcNow;
    }

    public static Result<ServiceRequestImage> Create(
        Guid id,
        Guid serviceRequestId,
        string imageKey)
    {
       

        return new ServiceRequestImage(
            id,
            serviceRequestId,
            imageKey);
    }
}
