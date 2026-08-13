using System.Linq;
using ApplicationTechnicianAvailabilityResponse =
    FixNow.Application.Features.TechnicianProfiles.Dtos.Responses.TechnicianAvailabilityResponse;
using ApplicationTechnicianAvailabilitySettingsResponse =
    FixNow.Application.Features.TechnicianProfiles.Dtos.Responses.TechnicianAvailabilitySettingsResponse;
using ApplicationTechnicianWorkingDayResponse =
    FixNow.Application.Features.TechnicianProfiles.Dtos.Responses.TechnicianWorkingDayResponse;
using ContractTechnicianAvailabilityResponse =
    FixNow.Contracts.Responses.TechnicianAvailabilityResponse;
using ContractTechnicianAvailabilitySettingsResponse =
    FixNow.Contracts.Responses.TechnicianAvailabilitySettingsResponse;

namespace FixNow.Api.Mappings.TechnicianAvailability;

public static class TechnicianAvailabilityMapping
{
    public static ContractTechnicianAvailabilityResponse ToContractResponse(
        this ApplicationTechnicianAvailabilityResponse response)
        => new(
            TechnicianProfileId: response.TechnicianProfileId,
            Availability: response.Availability);

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
