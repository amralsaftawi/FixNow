public sealed class ServiceRequest : AuditableEntity
{
    public Guid CustomerProfileId { get; private set; }

    public Guid AddressId { get; private set; }

    public Guid ServiceCategoryId { get; private set; }

    public string Description { get; private set; }

    public ServicePriority Priority { get; private set; }

    public ServiceRequestStatus Status { get; private set; }

    public DateTimeOffset RequestedAt { get; private set; }

    public DateTimeOffset? ScheduledAt { get; private set; }

    public DateTimeOffset? CompletedAt { get; private set; }

    public DateTimeOffset? CancelledAt { get; private set; }

    public CancellationReason? CancellationReason { get; private set; }

    // Navigation

    public CustomerProfile CustomerProfile { get; private set; } = null!;

    public Address Address { get; private set; } = null!;

    public ServiceCategory ServiceCategory { get; private set; } = null!;

    private readonly List<ServiceRequestImage> _images = [];

    private readonly List<ServiceRequestTimeline> _timeline = [];

    public IReadOnlyCollection<ServiceRequestImage> Images =>
        _images.AsReadOnly();

    public IReadOnlyCollection<ServiceRequestTimeline> Timeline =>
        _timeline.AsReadOnly();

#pragma warning disable CS8618
    private ServiceRequest()
    {
    }
#pragma warning disable CS8618
    private ServiceRequest(
        Guid id,
        Guid customerProfileId,
        Guid addressId,
        Guid serviceCategoryId,
        string description,
        ServicePriority priority,
        DateTimeOffset? scheduledAt)
        : base(id)
    {
        CustomerProfileId = customerProfileId;

        AddressId = addressId;

        ServiceCategoryId = serviceCategoryId;

        Description = description;

        Priority = priority;

        ScheduledAt = scheduledAt;

        RequestedAt = DateTimeOffset.UtcNow;

        Status = ServiceRequestStatus.Pending;
    }

    public static Result<ServiceRequest> Create(
        Guid id,
        Guid customerProfileId,
        Guid addressId,
        Guid serviceCategoryId,
        string description,
        ServicePriority priority,
        DateTimeOffset? scheduledAt)
    {
        if (id == Guid.Empty)
            return ServiceRequestErrors.IdRequired;

        if (customerProfileId == Guid.Empty)
            return ServiceRequestErrors.CustomerProfileIdRequired;

        if (addressId == Guid.Empty)
            return ServiceRequestErrors.AddressIdRequired;

        if (serviceCategoryId == Guid.Empty)
            return ServiceRequestErrors.ServiceCategoryIdRequired;

        if (string.IsNullOrWhiteSpace(description))
            return ServiceRequestErrors.DescriptionRequired;

        description = description.Trim();

        if (description.Length > 2000)
            return ServiceRequestErrors.DescriptionTooLong;

        if (scheduledAt.HasValue &&
            scheduledAt.Value <= DateTimeOffset.UtcNow)
        {
            return ServiceRequestErrors.InvalidScheduleDate;
        }

        var request = new ServiceRequest(
            id,
            customerProfileId,
            addressId,
            serviceCategoryId,
            description,
            priority,
            scheduledAt);

        request.AddDomainEvent(
            new ServiceRequestCreatedDomainEvent(
                request.Id,
                request.CustomerProfileId));

        return request;
    }

    public Result<Success> UpdateDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            return ServiceRequestErrors.DescriptionRequired;

        description = description.Trim();

        if (description.Length > 2000)
            return ServiceRequestErrors.DescriptionTooLong;

        if (Description == description)
            return ServiceRequestErrors.SameDescription;

        Description = description;

        AddDomainEvent(
            new ServiceRequestDescriptionUpdatedDomainEvent(Id));

        return Result.Success;
    }

    public Result<Success> ChangePriority(ServicePriority priority)
    {
        if (Priority == priority)
            return ServiceRequestErrors.SamePriority;

        Priority = priority;

        AddDomainEvent(
            new ServiceRequestPriorityChangedDomainEvent(
                Id,
                priority));

        return Result.Success;
    }

    public Result<Success> Schedule(DateTimeOffset scheduledAt)
    {
        if (scheduledAt <= DateTimeOffset.UtcNow)
            return ServiceRequestErrors.InvalidScheduleDate;

        ScheduledAt = scheduledAt;

        AddDomainEvent(
            new ServiceRequestScheduledDomainEvent(
                Id,
                scheduledAt));

        return Result.Success;
    }

    public Result<Success> MarkSearching()
    {
        if (Status != ServiceRequestStatus.Pending)
            return ServiceRequestErrors.InvalidStatusTransition;

        Status = ServiceRequestStatus.SearchingTechnician;

        _timeline.Add(
            ServiceRequestTimeline.Create(
                Guid.NewGuid(),
                Id,
                Status,
                "Searching for technician").Value);

        AddDomainEvent(
            new ServiceRequestSearchingDomainEvent(Id));

        return Result.Success;
    }

    public Result<Success> Complete()
    {
        if (Status == ServiceRequestStatus.Completed)
            return ServiceRequestErrors.AlreadyCompleted;

        Status = ServiceRequestStatus.Completed;

        CompletedAt = DateTimeOffset.UtcNow;

        _timeline.Add(
            ServiceRequestTimeline.Create(
                Guid.NewGuid(),
                Id,
                Status,
                "Service completed").Value);

        AddDomainEvent(
            new ServiceRequestCompletedDomainEvent(
                Id,
                CompletedAt.Value));

        return Result.Success;
    }

    public Result<Success> Cancel(CancellationReason reason)
    {
        if (Status == ServiceRequestStatus.Cancelled)
            return ServiceRequestErrors.AlreadyCancelled;

        Status = ServiceRequestStatus.Cancelled;

        CancelledAt = DateTimeOffset.UtcNow;

        CancellationReason = reason;

        _timeline.Add(
            ServiceRequestTimeline.Create(
                Guid.NewGuid(),
                Id,
                Status,
                $"Cancelled ({reason})").Value);

        AddDomainEvent(
            new ServiceRequestCancelledDomainEvent(
                Id,
                reason));

        return Result.Success;
    }

    public Result<Success> AddImage(ServiceRequestImage image)
    {
        if (_images.Any(x => x.ImageKey == image.ImageKey))
            return ServiceRequestErrors.ImageAlreadyAdded;

        _images.Add(image);

        AddDomainEvent(
            new ServiceRequestImageAddedDomainEvent(
                Id,
                image.Id));

        return Result.Success;
    }

    public Result<Success> RemoveImage(Guid imageId)
    {
        var image = _images.FirstOrDefault(x => x.Id == imageId);

        if (image is null)
            return ServiceRequestErrors.ImageNotFound;

        _images.Remove(image);

        AddDomainEvent(
            new ServiceRequestImageRemovedDomainEvent(
                Id,
                imageId));

        return Result.Success;
    }
}