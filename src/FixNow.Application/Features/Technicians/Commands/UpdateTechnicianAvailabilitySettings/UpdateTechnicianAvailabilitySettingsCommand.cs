using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Features.TechnicianProfiles.Dtos.Responses;

namespace FixNow.Application.Features.TechnicianProfiles.Commands.UpdateTechnicianAvailabilitySettings;

public sealed record UpdateTechnicianAvailabilitySettingsCommand(
    TechnicianAvailabilityStatus Status,
    IReadOnlyCollection<TechnicianWorkingDayCommand> WorkingDays,
    DateOnly? VacationStartDate,
    DateOnly? VacationEndDate)
    : ICommand<Result<TechnicianAvailabilitySettingsResponse>>;

public sealed record TechnicianWorkingDayCommand(
    WorkingDayOfWeek Day,
    TimeOnly StartTime,
    TimeOnly EndTime);
