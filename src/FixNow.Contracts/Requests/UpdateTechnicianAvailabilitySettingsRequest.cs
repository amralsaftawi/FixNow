namespace FixNow.Contracts.Requests;

public sealed record UpdateTechnicianAvailabilitySettingsRequest
{
    public TechnicianAvailabilityStatus Status { get; init; }

    public List<WorkingDayRequest> WorkingDays { get; init; } = [];

    public DateOnly? VacationStartDate { get; init; }

    public DateOnly? VacationEndDate { get; init; }
}

public sealed record WorkingDayRequest(
    WorkingDayOfWeek Day,
    TimeOnly StartTime,
    TimeOnly EndTime);
