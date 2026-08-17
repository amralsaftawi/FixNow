public sealed class Job : AuditableEntity
{
    public Guid ServiceRequestId { get; private set; }

    public Guid TechnicianProfileId { get; private set; }

    public JobStatus Status { get; private set; }

    public bool IsTerminated =>
        _allowedTransitions[Status].Length == 0;

    public uint Version { get; private set; }

    public DateTimeOffset? CompletionConfirmedAtUtc { get; private set; }

    // Price snapshot: captured once when the job is completed so a historical
    // completed job never changes price when the technician service price or
    // the service category inspection fee are reconfigured later.

    public Money? ServicePrice { get; private set; }

    public Money? InspectionFee { get; private set; }

    // Navigation

    public ServiceRequest ServiceRequest { get; private set; } = null!;

    public TechnicianProfile TechnicianProfile { get; private set; } = null!;

    private readonly List<JobTimeline> _timeline = [];

    public IReadOnlyCollection<JobTimeline> Timeline =>
        _timeline.AsReadOnly();

    private readonly List<JobAdditionalCharge> _additionalCharges = [];

    public IReadOnlyCollection<JobAdditionalCharge> AdditionalCharges =>
        _additionalCharges.AsReadOnly();

#pragma warning disable CS8618
    private Job()
    {
    }
#pragma warning restore CS8618
    private Job(
        Guid id,
        Guid serviceRequestId,
        Guid technicianProfileId)
        : base(id)
    {
        ServiceRequestId = serviceRequestId;

        TechnicianProfileId = technicianProfileId;

        Status = JobStatus.Created;
    }

    public static Result<Job> Create(
        Guid id,
        Guid serviceRequestId,
        Guid technicianProfileId)
    {
        if (id == Guid.Empty)
            return JobErrors.IdRequired;

        if (serviceRequestId == Guid.Empty)
            return JobErrors.ServiceRequestIdRequired;

        if (technicianProfileId == Guid.Empty)
            return JobErrors.TechnicianProfileIdRequired;

        var job = new Job(
            id,
            serviceRequestId,
            technicianProfileId);

        job.AddDomainEvent(
            new JobCreatedDomainEvent(
                job.Id,
                job.ServiceRequestId,
                job.TechnicianProfileId));

        job._timeline.Add(
            JobTimeline.Create(
                Guid.NewGuid(),
                job.Id,
                JobStatus.Created,
                "Job created").Value);

        return job;
    }

    public Result<Success> ChangeStatus(JobStatus newStatus)
    {
        if (Status == newStatus)
        {
            return JobErrors.SameStatus;
        }

        if (!_allowedTransitions.TryGetValue(Status, out var allowed) ||
            !allowed.Contains(newStatus))
        {
            return JobErrors.InvalidStatusTransition;
        }

        var previousStatus = Status;

        Status = newStatus;

        AddDomainEvent(
            new JobStatusChangedDomainEvent(
                Id,
                ServiceRequestId,
                TechnicianProfileId,
                previousStatus,
                newStatus));

        _timeline.Add(
            JobTimeline.Create(
                Guid.NewGuid(),
                Id,
                newStatus,
                GetTimelineDescription(previousStatus, newStatus)).Value);

        return Result.Success;
    }

    public Result<Success> ConfirmCompletion()
    {
        // Confirmation is a business state associated with completion, not a
        // status change. The Job remains Completed; only a completed Job can
        // be confirmed, and only once.
        if (Status != JobStatus.Completed)
        {
            return JobErrors.JobNotCompleted;
        }

        if (CompletionConfirmedAtUtc.HasValue)
        {
            return JobErrors.CompletionAlreadyConfirmed;
        }

        CompletionConfirmedAtUtc = DateTimeOffset.UtcNow;

        AddDomainEvent(
            new JobCompletionConfirmedDomainEvent(
                Id,
                ServiceRequestId,
                TechnicianProfileId,
                CompletionConfirmedAtUtc.Value));

        _timeline.Add(
            JobTimeline.Create(
                Guid.NewGuid(),
                Id,
                Status,
                "Service completion confirmed by customer").Value);

        return Result.Success;
    }

    public Result<Success> AddAdditionalCharge(
        JobAdditionalCharge charge)
    {
        if (charge is null)
        {
            return JobErrors.AdditionalChargeRequired;
        }

        // The job lifecycle treats Completed and Cancelled as immutable
        // terminal states; no further charges may be recorded against them.
        // Charges are therefore only allowed while the job is active
        // (Created, OnTheWay, Arrived, InProgress, Paused).
        if (IsTerminated)
        {
            return JobErrors.AdditionalChargeNotAllowed;
        }

        _additionalCharges.Add(charge);

        AddDomainEvent(
            new JobAdditionalChargeAddedDomainEvent(
                Id,
                charge.Id,
                charge.Description,
                charge.Amount));

        return Result.Success;
    }

    public Result<Success> FinalizePrice(
        Money? servicePrice,
        Money? inspectionFee)
    {
        // The final price becomes authoritative only when the job is
        // completed: at that point no further additional charges can be
        // recorded, and the resolved service price and inspection fee are
        // snapshotted so later pricing configuration changes can never alter
        // a completed job's price.
        if (Status != JobStatus.Completed)
        {
            return JobErrors.FinalPriceNotAllowed;
        }

        ServicePrice = servicePrice;

        InspectionFee = inspectionFee;

        return Result.Success;
    }

    private static readonly IReadOnlyDictionary<JobStatus, JobStatus[]> _allowedTransitions =
        new Dictionary<JobStatus, JobStatus[]>
        {
            [JobStatus.Created] =
                [JobStatus.OnTheWay, JobStatus.Arrived, JobStatus.InProgress, JobStatus.Cancelled],
            [JobStatus.OnTheWay] =
                [JobStatus.Arrived, JobStatus.Cancelled],
            [JobStatus.Arrived] =
                [JobStatus.InProgress, JobStatus.Cancelled],
            [JobStatus.InProgress] =
                [JobStatus.Paused, JobStatus.Completed, JobStatus.Cancelled],
            [JobStatus.Paused] =
                [JobStatus.InProgress, JobStatus.Cancelled],
            [JobStatus.Completed] = [],
            [JobStatus.Cancelled] = []
        };

    private static string GetTimelineDescription(
        JobStatus previousStatus,
        JobStatus newStatus)
        => (previousStatus, newStatus) switch
        {
            (_, JobStatus.Completed) => "Job completed",
            (_, JobStatus.Cancelled) => "Job cancelled",
            (JobStatus.Paused, JobStatus.InProgress) => "Job resumed",
            (_, JobStatus.InProgress) => "Job started",
            (_, JobStatus.OnTheWay) => "Technician is on the way",
            (_, JobStatus.Arrived) => "Technician arrived",
            (_, JobStatus.Paused) => "Job paused",
            (_, _) => $"Job status changed to {newStatus}"
        };
}
