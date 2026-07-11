public sealed class TechnicianService : AuditableEntity
{
    public Guid TechnicianProfileId { get; private set; }

    public Guid ServiceCategoryId { get; private set; }

    // Navigation

    public TechnicianProfile TechnicianProfile { get; private set; } = null!;

    public ServiceCategory ServiceCategory { get; private set; } = null!;

#pragma warning disable CS8618
    private TechnicianService()
    {
    }
#pragma warning disable CS8618
    private TechnicianService(
        Guid id,
        Guid technicianProfileId,
        Guid serviceCategoryId)
        : base(id)
    {
        TechnicianProfileId = technicianProfileId;
        ServiceCategoryId = serviceCategoryId;
    }


    public static Result<TechnicianService> Create(
    Guid id,
    Guid technicianProfileId,
    Guid serviceCategoryId)
{
    if (id == Guid.Empty)
        return TechnicianServiceErrors.IdRequired;

    if (technicianProfileId == Guid.Empty)
        return TechnicianServiceErrors.TechnicianProfileIdRequired;

    if (serviceCategoryId == Guid.Empty)
        return TechnicianServiceErrors.ServiceCategoryIdRequired;

    return new TechnicianService(
        id,
        technicianProfileId,
        serviceCategoryId);
}
}