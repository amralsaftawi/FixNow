public sealed class CustomerRating : AuditableEntity
{
    public Guid JobId { get; private set; }

    public Guid TechnicianProfileId { get; private set; }

    public Guid CustomerProfileId { get; private set; }

    public CustomerRatingScore Rating { get; private set; }

    public string? Comment { get; private set; }

    // Navigation

    public Job Job { get; private set; } = null!;

    public TechnicianProfile TechnicianProfile { get; private set; } = null!;

    public CustomerProfile CustomerProfile { get; private set; } = null!;

#pragma warning disable CS8618
    private CustomerRating()
    {
    }
#pragma warning disable CS8618
    private CustomerRating(
        Guid id,
        Guid jobId,
        Guid technicianProfileId,
        Guid customerProfileId,
        CustomerRatingScore rating,
        string? comment)
        : base(id)
    {
        JobId = jobId;
        TechnicianProfileId = technicianProfileId;
        CustomerProfileId = customerProfileId;
        Rating = rating;
        Comment = comment;
    }

    public static Result<CustomerRating> Create(
        Guid id,
        Guid jobId,
        Guid technicianProfileId,
        Guid customerProfileId,
        CustomerRatingScore rating,
        string? comment = null)
    {
        if (id == Guid.Empty)
            return CustomerRatingErrors.IdRequired;

        if (jobId == Guid.Empty)
            return CustomerRatingErrors.JobIdRequired;

        if (technicianProfileId == Guid.Empty)
            return CustomerRatingErrors.TechnicianProfileIdRequired;

        if (customerProfileId == Guid.Empty)
            return CustomerRatingErrors.CustomerProfileIdRequired;

        var customerRating = new CustomerRating(
            id,
            jobId,
            technicianProfileId,
            customerProfileId,
            rating,
            comment);

        customerRating.AddDomainEvent(
            new CustomerRatingCreatedDomainEvent(
                customerRating.Id,
                customerRating.CustomerProfileId));

        return customerRating;
    }
}
