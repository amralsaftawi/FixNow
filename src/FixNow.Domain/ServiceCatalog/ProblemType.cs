
public sealed class ProblemType : AuditableEntity
{
    public string Name { get; private set; }

    public Guid ServiceCategoryId { get; private set; }

    public bool IsActive { get; private set; }

    // Navigation

    public ServiceCategory ServiceCategory { get; private set; } = null!;

#pragma warning disable CS8618
    private ProblemType()
    {
    }
#pragma warning disable CS8618
    private ProblemType(
        Guid id,
        string name,
        Guid serviceCategoryId)
        : base(id)
    {
        Name = name;
        ServiceCategoryId = serviceCategoryId;
        IsActive = true;
    }

    public static Result<ProblemType> Create(
        Guid id,
        string name,
        Guid serviceCategoryId)
    {
        if (id == Guid.Empty)
            return ProblemTypeErrors.IdRequired;

        if (string.IsNullOrWhiteSpace(name))
            return ProblemTypeErrors.NameRequired;

        name = name.Trim();

        if (name.Length > 100)
            return ProblemTypeErrors.NameTooLong;

        if (serviceCategoryId == Guid.Empty)
            return ProblemTypeErrors.ServiceCategoryIdRequired;

        var problemType = new ProblemType(
            id,
            name,
            serviceCategoryId);

        problemType.AddDomainEvent(
            new ProblemTypeCreatedDomainEvent(problemType.Id));

        return problemType;
    }
}
