public sealed class Assignment : AuditableEntity
{
    public Guid ServiceRequestId { get; private set; }

    public Guid TechnicianProfileId { get; private set; }

    public AssignmentStatus Status { get; private set; }

    public DateTimeOffset AssignedAt { get; private set; }

    public DateTimeOffset? AcceptedAt { get; private set; }

    public DateTimeOffset? RejectedAt { get; private set; }

    public DateTimeOffset? CompletedAt { get; private set; }

    public AssignmentRejectReason? RejectReason { get; private set; }

    // Navigation

    public ServiceRequest ServiceRequest { get; private set; } = null!;

    public TechnicianProfile TechnicianProfile { get; private set; } = null!;

#pragma warning disable CS8618
    private Assignment()
    {
    }
#pragma warning disable CS8618
    private Assignment(
        Guid id,
        Guid serviceRequestId,
        Guid technicianProfileId)
        : base(id)
    {
        ServiceRequestId = serviceRequestId;

        TechnicianProfileId = technicianProfileId;

        AssignedAt = DateTimeOffset.UtcNow;

        Status = AssignmentStatus.Pending;
    }

    public static Result<Assignment> Create(
        Guid id,
        Guid serviceRequestId,
        Guid technicianProfileId)
    {
        if (id == Guid.Empty)
            return AssignmentErrors.IdRequired;

        if (serviceRequestId == Guid.Empty)
            return AssignmentErrors.ServiceRequestIdRequired;

        if (technicianProfileId == Guid.Empty)
            return AssignmentErrors.TechnicianProfileIdRequired;

        var assignment = new Assignment(
            id,
            serviceRequestId,
            technicianProfileId);

        assignment.AddDomainEvent(
            new AssignmentCreatedDomainEvent(
                assignment.Id,
                assignment.ServiceRequestId,
                assignment.TechnicianProfileId));

        return assignment;
    }


    public Result<Success> Accept()
{
    if (Status == AssignmentStatus.Accepted)
        return AssignmentErrors.AlreadyAccepted;

    if (Status != AssignmentStatus.Pending)
        return AssignmentErrors.InvalidStatusTransition;

    Status = AssignmentStatus.Accepted;

    AcceptedAt = DateTimeOffset.UtcNow;

    AddDomainEvent(
        new AssignmentAcceptedDomainEvent(
            Id,
            ServiceRequestId,
            TechnicianProfileId));

    return Result.Success;
}


public Result<Success> Reject(
    AssignmentRejectReason reason)
{
    if (Status == AssignmentStatus.Rejected)
        return AssignmentErrors.AlreadyRejected;

    if (Status != AssignmentStatus.Pending)
        return AssignmentErrors.InvalidStatusTransition;

    Status = AssignmentStatus.Rejected;

    RejectReason = reason;

    RejectedAt = DateTimeOffset.UtcNow;

    AddDomainEvent(
        new AssignmentRejectedDomainEvent(
            Id,
            ServiceRequestId,
            TechnicianProfileId,
            reason));

    return Result.Success;
}


public Result<Success> Complete()
{
    if (Status == AssignmentStatus.Completed)
        return AssignmentErrors.AlreadyCompleted;

    if (Status != AssignmentStatus.Accepted)
        return AssignmentErrors.InvalidStatusTransition;

    Status = AssignmentStatus.Completed;

    CompletedAt = DateTimeOffset.UtcNow;

    AddDomainEvent(
        new AssignmentCompletedDomainEvent(
            Id,
            ServiceRequestId,
            TechnicianProfileId,
            CompletedAt.Value));

    return Result.Success;
}


public Result<Success> Cancel()
{
    if (Status == AssignmentStatus.Cancelled)
        return AssignmentErrors.AlreadyCancelled;

    if (Status == AssignmentStatus.Completed)
        return AssignmentErrors.InvalidStatusTransition;

    Status = AssignmentStatus.Cancelled;

    AddDomainEvent(
        new AssignmentCancelledDomainEvent(
            Id,
            ServiceRequestId,
            TechnicianProfileId));

    return Result.Success;
}
}