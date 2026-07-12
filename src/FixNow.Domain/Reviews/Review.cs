public sealed class Review : AuditableEntity
{
    public Guid AssignmentId { get; private set; }

    public Guid ServiceRequestId { get; private set; }

    public Guid CustomerProfileId { get; private set; }

    public Guid TechnicianProfileId { get; private set; }

    public Rating Rating { get; private set; }

    public string? Comment { get; private set; }

    // Navigation

    public Assignment Assignment { get; private set; } = null!;

    public ServiceRequest ServiceRequest { get; private set; } = null!;

    public CustomerProfile CustomerProfile { get; private set; } = null!;

    public TechnicianProfile TechnicianProfile { get; private set; } = null!;

#pragma warning disable CS8618
    private Review()
    {
    }
#pragma warning disable CS8618
    private Review(
        Guid id,
        Guid assignmentId,
        Guid serviceRequestId,
        Guid customerProfileId,
        Guid technicianProfileId,
        Rating rating,
        string? comment)
        : base(id)
    {
        AssignmentId = assignmentId;

        ServiceRequestId = serviceRequestId;

        CustomerProfileId = customerProfileId;

        TechnicianProfileId = technicianProfileId;

        Rating = rating;

        Comment = comment;
    }

    public static Result<Review> Create(
        Guid id,
        Guid assignmentId,
        Guid serviceRequestId,
        Guid customerProfileId,
        Guid technicianProfileId,
        Rating rating,
        string? comment)
    {
        if (id == Guid.Empty)
            return ReviewErrors.IdRequired;

        if (assignmentId == Guid.Empty)
            return ReviewErrors.AssignmentIdRequired;

        if (serviceRequestId == Guid.Empty)
            return ReviewErrors.ServiceRequestIdRequired;

        if (customerProfileId == Guid.Empty)
            return ReviewErrors.CustomerProfileIdRequired;

        if (technicianProfileId == Guid.Empty)
            return ReviewErrors.TechnicianProfileIdRequired;

        comment = comment?.Trim();

        if (comment?.Length > 1000)
            return ReviewErrors.CommentTooLong;

        var review = new Review(
            id,
            assignmentId,
            serviceRequestId,
            customerProfileId,
            technicianProfileId,
            rating,
            comment);

        review.AddDomainEvent(
            new ReviewCreatedDomainEvent(
                review.Id,
                review.AssignmentId,
                review.TechnicianProfileId));

        return review;
    }

public Result<Success> Update(Rating rating,  string? comment)
{
    comment = comment?.Trim();

    if (comment?.Length > 1000)
        return ReviewErrors.CommentTooLong;

    if (Rating == rating && Comment == comment)
        return ReviewErrors.NothingChanged;

    Rating = rating;

    Comment = comment;

 AddDomainEvent(
    new ReviewUpdatedDomainEvent(
        Id,
        AssignmentId,
        TechnicianProfileId));
    return Result.Success;
}
}