public sealed class TechnicianAvailabilitySettings : ValueObject
{
    private readonly List<TechnicianWorkingDay> _workingDays = [];

    public TechnicianAvailabilityStatus Status { get; private set; }

    public IReadOnlyCollection<TechnicianWorkingDay> WorkingDays =>
        _workingDays.AsReadOnly();

    public DateOnly? VacationStartDate { get; private set; }

    public DateOnly? VacationEndDate { get; private set; }

    public static Result<TechnicianAvailabilitySettings> Create(
        TechnicianAvailabilityStatus status,
        IReadOnlyCollection<TechnicianWorkingDay>? workingDays,
        DateOnly? vacationStartDate,
        DateOnly? vacationEndDate)
    {
        if (!Enum.IsDefined(status))
            return TechnicianAvailabilityErrors.InvalidStatus;

        if (workingDays is null)
            return TechnicianAvailabilityErrors.WorkingDayRequired;

        if (vacationStartDate is not null && vacationEndDate is null
            || vacationStartDate is null && vacationEndDate is not null)
            return TechnicianAvailabilityErrors.VacationDatesBothRequired;

        if (vacationStartDate is not null
            && vacationEndDate is not null
            && vacationEndDate < vacationStartDate)
            return TechnicianAvailabilityErrors.InvalidVacationRange;

        if (workingDays
            .GroupBy(workingDay => workingDay.Day)
            .Any(group => group.Count() > 1))
            return TechnicianAvailabilityErrors.DuplicateWorkingDay;

        return new TechnicianAvailabilitySettings(
            status,
            workingDays,
            vacationStartDate,
            vacationEndDate);
    }

    private TechnicianAvailabilitySettings()
    {
    }

    private TechnicianAvailabilitySettings(
        TechnicianAvailabilityStatus status,
        IReadOnlyCollection<TechnicianWorkingDay> workingDays,
        DateOnly? vacationStartDate,
        DateOnly? vacationEndDate)
    {
        Status = status;
        _workingDays.AddRange(workingDays);
        VacationStartDate = vacationStartDate;
        VacationEndDate = vacationEndDate;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Status;
        yield return VacationStartDate;
        yield return VacationEndDate;

        foreach (var workingDay in WorkingDays)
            yield return workingDay;
    }
}
