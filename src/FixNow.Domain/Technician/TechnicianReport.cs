public sealed class TechnicianReport : AuditableEntity
{
    public Guid TechnicianProfileId { get; private set; }

    public Guid ReporterUserId { get; private set; }

    public TechnicianReportReason Reason { get; private set; }

    public string? Description { get; private set; }

    public TechnicianReportStatus Status { get; private set; }

    // Navigation

    public TechnicianProfile TechnicianProfile { get; private set; } = null!;

    public User ReporterUser { get; private set; } = null!;

#pragma warning disable CS8618
    private TechnicianReport()
    {
    }
#pragma warning disable CS8618
    private TechnicianReport(
        Guid id,
        Guid technicianProfileId,
        Guid reporterUserId,
        TechnicianReportReason reason,
        string? description)
        : base(id)
    {
        TechnicianProfileId = technicianProfileId;
        ReporterUserId = reporterUserId;
        Reason = reason;
        Description = description;
        Status = TechnicianReportStatus.Pending;
    }

    public static Result<TechnicianReport> Create(
        Guid id,
        Guid technicianProfileId,
        Guid reporterUserId,
        TechnicianReportReason reason,
        string? description = null)
    {
        if (id == Guid.Empty)
            return TechnicianReportErrors.IdRequired;

        if (technicianProfileId == Guid.Empty)
            return TechnicianReportErrors.TechnicianProfileIdRequired;

        if (reporterUserId == Guid.Empty)
            return TechnicianReportErrors.ReporterUserIdRequired;

        description = description?.Trim();

        if (description?.Length > 1000)
            return TechnicianReportErrors.DescriptionTooLong;

        var report = new TechnicianReport(
            id,
            technicianProfileId,
            reporterUserId,
            reason,
            description);

        report.AddDomainEvent(
            new TechnicianReportedDomainEvent(
                report.Id,
                report.TechnicianProfileId,
                report.ReporterUserId));

        return report;
    }
}
