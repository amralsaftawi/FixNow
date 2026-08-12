namespace FixNow.Application.Features.TechnicianProfiles.Dtos.Responses;

public sealed record TechnicianAvailabilitySettingsResponse(
    Guid TechnicianProfileId,
    TechnicianAvailabilityStatus Status,
    IReadOnlyCollection<TechnicianWorkingDayResponse> WorkingDays,
    DateOnly? VacationStartDate,
    DateOnly? VacationEndDate);

public sealed record TechnicianWorkingDayResponse(
    WorkingDayOfWeek Day,
    TimeOnly StartTime,
    TimeOnly EndTime);
