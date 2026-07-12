public sealed class Payment : AuditableEntity
{
    public Guid AssignmentId { get; private set; }

    public Guid CustomerProfileId { get; private set; }

    public Money Amount { get; private set; }

    public PaymentMethod PaymentMethod { get; private set; }

    public PaymentStatus Status { get; private set; }

    public DateTimeOffset? PaidAt { get; private set; }

    // Navigation

    public Assignment Assignment { get; private set; } = null!;

    public CustomerProfile CustomerProfile { get; private set; } = null!;

    private Payment()
    {
    }

    private Payment(
        Guid id,
        Guid assignmentId,
        Guid customerProfileId,
        Money amount,
        PaymentMethod paymentMethod)
        : base(id)
    {
        AssignmentId = assignmentId;

        CustomerProfileId = customerProfileId;

        Amount = amount;

        PaymentMethod = paymentMethod;

        Status = PaymentStatus.Pending;
    }

    public static Result<Payment> Create(
        Guid id,
        Guid assignmentId,
        Guid customerProfileId,
        Money amount,
        PaymentMethod paymentMethod)
    {
        if (id == Guid.Empty)
            return PaymentErrors.IdRequired;

        if (assignmentId == Guid.Empty)
            return PaymentErrors.AssignmentIdRequired;

        if (customerProfileId == Guid.Empty)
            return PaymentErrors.CustomerProfileIdRequired;

        var payment = new Payment(
            id,
            assignmentId,
            customerProfileId,
            amount,
            paymentMethod);

       payment.AddDomainEvent(
    new PaymentCreatedDomainEvent(
        payment.Id,
        payment.AssignmentId,
        payment.CustomerProfileId,
        payment.Amount,
        payment.PaymentMethod));

        return payment;
    }

public Result<Success> MarkAsPaid()
{
    if (Status == PaymentStatus.Paid)
        return PaymentErrors.AlreadyPaid;

    if (Status != PaymentStatus.Pending)
        return PaymentErrors.InvalidStatusTransition;

    Status = PaymentStatus.Paid;

    PaidAt = DateTimeOffset.UtcNow;
AddDomainEvent(
    new PaymentSucceededDomainEvent(
        Id,
        AssignmentId,
        CustomerProfileId,
        Amount,
        PaidAt!.Value));

    return Result.Success;
}

    public Result<Success> MarkAsFailed()
{
    if (Status == PaymentStatus.Failed)
        return PaymentErrors.AlreadyFailed;

    if (Status != PaymentStatus.Pending)
        return PaymentErrors.InvalidStatusTransition;

    Status = PaymentStatus.Failed;

   AddDomainEvent(
    new PaymentFailedDomainEvent(
        Id,
        AssignmentId,
        CustomerProfileId));

    return Result.Success;
}

public Result<Success> MarkAsRefunded()
{
    if (Status == PaymentStatus.Refunded)
        return PaymentErrors.AlreadyRefunded;

    if (Status != PaymentStatus.Paid)
        return PaymentErrors.InvalidStatusTransition;

    Status = PaymentStatus.Refunded;

   AddDomainEvent(
    new PaymentRefundedDomainEvent(
        Id,
        AssignmentId,
        CustomerProfileId,
        Amount));
    return Result.Success;
}
}