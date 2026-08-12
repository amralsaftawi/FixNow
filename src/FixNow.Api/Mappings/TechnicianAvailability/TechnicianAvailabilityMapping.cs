using System.Linq;
using ApplicationTechnicianAvailabilitySettingsResponse =
    FixNow.Application.Features.TechnicianProfiles.Dtos.Responses.TechnicianAvailabilitySettingsResponse;
using ApplicationTechnicianWorkingDayResponse =
    FixNow.Application.Features.TechnicianProfiles.Dtos.Responses.TechnicianWorkingDayResponse;
using ContractTechnicianAvailabilitySettingsResponse =
    FixNow.Contracts.Responses.TechnicianAvailabilitySettingsResponse;

namespace FixNow.Api.Mappings.TechnicianAvailability;

public static class TechnicianAvailabilityMapping
{
    public static ContractTechnicianAvailabilitySettingsResponse ToContractResponse(
        this ApplicationTechnicianAvailabilitySettingsResponse response)
        => new(
            TechnicianProfileId: response.TechnicianProfileId,
            Status: response.Status,
            WorkingDays: response.WorkingDays
                .Select(ToContractResponse)
                .ToList(),
            VacationStartDate: response.VacationStartDate,
            VacationEndDate: response.VacationEndDate);

    private static FixNow.Contracts.Responses.TechnicianWorkingDayResponse ToContractResponse(
        ApplicationTechnicianWorkingDayResponse response)
        => new(
            Day: response.Day,
            StartTime: response.StartTime,
            EndTime: response.EndTime);
}
