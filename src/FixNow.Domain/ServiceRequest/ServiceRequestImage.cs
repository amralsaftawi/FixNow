public sealed class ServiceRequestImage : AuditableEntity
{
    public Guid ServiceRequestId { get; private set; }

    public string ImageKey { get; private set; }

    // Navigation

    public ServiceRequest ServiceRequest { get; private set; } = null!;

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
    }

    public static Result<ServiceRequestImage> Create(
        Guid id,
        Guid serviceRequestId,
        string imageKey)
    {
        if (id == Guid.Empty)
            return ServiceRequestImageErrors.IdRequired;

        if (serviceRequestId == Guid.Empty)
            return ServiceRequestImageErrors.ServiceRequestIdRequired;

        if (string.IsNullOrWhiteSpace(imageKey))
            return ServiceRequestImageErrors.ImageKeyRequired;

        imageKey = imageKey.Trim();

        return new ServiceRequestImage(
            id,
            serviceRequestId,
            imageKey);
    }
}