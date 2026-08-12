public sealed class TechnicianWorkingDay : ValueObject
{
    public WorkingDayOfWeek Day { get; private set; }

    public TimeOnly StartTime { get; private set; }

    public TimeOnly EndTime { get; private set; }

    public static Result<TechnicianWorkingDay> Create(
        WorkingDayOfWeek day,
        TimeOnly startTime,
        TimeOnly endTime)
    {
        if (!Enum.IsDefined(day))
            return TechnicianAvailabilityErrors.InvalidWorkingDay;

        if (endTime <= startTime)
            return TechnicianAvailabilityErrors.EndTimeNotAfterStartTime;

        return new TechnicianWorkingDay(day, startTime, endTime);
    }

    private TechnicianWorkingDay()
    {
    }

    private TechnicianWorkingDay(
        WorkingDayOfWeek day,
        TimeOnly startTime,
        TimeOnly endTime)
    {
        Day = day;
        StartTime = startTime;
        EndTime = endTime;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Day;
        yield return StartTime;
        yield return EndTime;
    }
}
