public sealed class TechnicianExperience : AuditableEntity
{
    public Guid TechnicianProfileId { get; private set; }

    public string CompanyName { get; private set; }

    public string Position { get; private set; }

    public string? Description { get; private set; }

    public DateTimeOffset StartDate { get; private set; }

    public DateTimeOffset? EndDate { get; private set; }

    public bool IsCurrent => EndDate is null;

    // Navigation

    public TechnicianProfile TechnicianProfile { get; private set; } = null!;

#pragma warning disable CS8618
    private TechnicianExperience()
    {
    }
#pragma warning disable CS8618
    private TechnicianExperience(
        Guid id,
        Guid technicianProfileId,
        string companyName,
        string position,
        string? description,
        DateTimeOffset startDate,
        DateTimeOffset? endDate)
        : base(id)
    {
        TechnicianProfileId = technicianProfileId;
        CompanyName = companyName;
        Position = position;
        Description = description;
        StartDate = startDate;
        EndDate = endDate;
    }

    public static Result<TechnicianExperience> Create(
        Guid id,
        Guid technicianProfileId,
        string companyName,
        string position,
        string? description,
        DateTimeOffset startDate,
        DateTimeOffset? endDate)
    {
        if (id == Guid.Empty)
            return TechnicianExperienceErrors.IdRequired;

        if (technicianProfileId == Guid.Empty)
            return TechnicianExperienceErrors.TechnicianProfileIdRequired;

        companyName = companyName?.Trim();

        if (string.IsNullOrWhiteSpace(companyName))
            return TechnicianExperienceErrors.CompanyNameRequired;

        if (companyName.Length > 150)
            return TechnicianExperienceErrors.CompanyNameTooLong;

        position = position?.Trim();

        if (string.IsNullOrWhiteSpace(position))
            return TechnicianExperienceErrors.PositionRequired;

        if (position.Length > 150)
            return TechnicianExperienceErrors.PositionTooLong;

        description = description?.Trim();

        if (description?.Length > 1000)
            return TechnicianExperienceErrors.DescriptionTooLong;

        if (startDate == default)
            return TechnicianExperienceErrors.StartDateRequired;

        if (endDate is not null && endDate <= startDate)
            return TechnicianExperienceErrors.EndDateBeforeStartDate;

        var experience = new TechnicianExperience(
            id,
            technicianProfileId,
            companyName,
            position,
            description,
            startDate,
            endDate);

        experience.AddDomainEvent(
            new TechnicianExperienceAddedDomainEvent(
                experience.Id,
                experience.TechnicianProfileId));

        return experience;
    }

    public Result<Success> Update(
        string companyName,
        string position,
        string? description,
        DateTimeOffset startDate,
        DateTimeOffset? endDate)
    {
        companyName = companyName?.Trim();

        if (string.IsNullOrWhiteSpace(companyName))
            return TechnicianExperienceErrors.CompanyNameRequired;

        if (companyName.Length > 150)
            return TechnicianExperienceErrors.CompanyNameTooLong;

        position = position?.Trim();

        if (string.IsNullOrWhiteSpace(position))
            return TechnicianExperienceErrors.PositionRequired;

        if (position.Length > 150)
            return TechnicianExperienceErrors.PositionTooLong;

        description = description?.Trim();

        if (description?.Length > 1000)
            return TechnicianExperienceErrors.DescriptionTooLong;

        if (startDate == default)
            return TechnicianExperienceErrors.StartDateRequired;

        if (endDate is not null && endDate <= startDate)
            return TechnicianExperienceErrors.EndDateBeforeStartDate;

        CompanyName = companyName;
        Position = position;
        Description = description;
        StartDate = startDate;
        EndDate = endDate;

        AddDomainEvent(
            new TechnicianExperienceUpdatedDomainEvent(
                Id,
                TechnicianProfileId));

        return Result.Success;
    }
}
