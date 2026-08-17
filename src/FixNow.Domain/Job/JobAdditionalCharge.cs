public sealed class JobAdditionalCharge : AuditableEntity
{
    public Guid JobId { get; private set; }

    public string Description { get; private set; }

    public Money Amount { get; private set; }

    // Navigation

    public Job Job { get; private set; } = null!;

#pragma warning disable CS8618
    private JobAdditionalCharge()
    {
    }
#pragma warning disable CS8618
    private JobAdditionalCharge(
        Guid id,
        Guid jobId,
        string description,
        Money amount)
        : base(id)
    {
        JobId = jobId;

        Description = description;

        Amount = amount;

        CreatedAtUtc = DateTimeOffset.UtcNow;
    }

    public static Result<JobAdditionalCharge> Create(
        Guid id,
        Guid jobId,
        string description,
        Money amount)
    {
        if (id == Guid.Empty)
            return JobAdditionalChargeErrors.IdRequired;

        if (jobId == Guid.Empty)
            return JobAdditionalChargeErrors.JobIdRequired;

        if (string.IsNullOrWhiteSpace(description))
            return JobAdditionalChargeErrors.DescriptionRequired;

        description = description.Trim();

        if (description.Length > 500)
            return JobAdditionalChargeErrors.DescriptionTooLong;

        if (amount is null)
            return JobAdditionalChargeErrors.AmountRequired;

        return new JobAdditionalCharge(
            id,
            jobId,
            description,
            amount);
    }
}
