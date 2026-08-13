using System.Linq;
using FixNow.Application.Features.TechnicianProfiles.Dtos.Responses;

namespace FixNow.Application.Features.TechnicianProfiles.Mappers;

public static class TechnicianAvailabilityMapping
{
    public static TechnicianAvailabilityResponse ToTechnicianAvailabilityResponse(
        this TechnicianProfile entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new TechnicianAvailabilityResponse(
            TechnicianProfileId: entity.Id,
            Availability: entity.Availability);
    }

    public static TechnicianAvailabilitySettingsResponse
        ToTechnicianAvailabilitySettingsResponse(
            this TechnicianProfile entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new TechnicianAvailabilitySettingsResponse(
            TechnicianProfileId: entity.Id,
            Status: entity.AvailabilitySettings.Status,
            WorkingDays: entity.AvailabilitySettings.WorkingDays
                .Select(workingDay => new TechnicianWorkingDayResponse(
                    workingDay.Day,
                    workingDay.StartTime,
                    workingDay.EndTime))
                .ToList(),
            VacationStartDate: entity.AvailabilitySettings.VacationStartDate,
            VacationEndDate: entity.AvailabilitySettings.VacationEndDate);
    }
}
