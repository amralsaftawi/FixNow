using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Application.Features.TechnicianProfiles.Dtos.Responses;
using FixNow.Application.Features.TechnicianProfiles.Mappers;

namespace FixNow.Application.Features.TechnicianProfiles.Commands.UpdateTechnicianAvailabilitySettings;

public sealed class UpdateTechnicianAvailabilitySettingsCommandHandler(
    ITechnicianProfileRepository technicianProfileRepository,
    ICurrentUser currentUser)
    : ICommandHandler<
        UpdateTechnicianAvailabilitySettingsCommand,
        Result<TechnicianAvailabilitySettingsResponse>>
{
    public async Task<Result<TechnicianAvailabilitySettingsResponse>> Handle(
        UpdateTechnicianAvailabilitySettingsCommand command,
        CancellationToken cancellationToken)
    {
        var technicianProfile = await technicianProfileRepository.GetByUserIdAsync(
            currentUser.UserId,
            cancellationToken);

        if (technicianProfile is null)
        {
            return TechnicianProfileErrors.NotFound;
        }

        var workingDays = new List<TechnicianWorkingDay>(command.WorkingDays.Count);

        foreach (var workingDay in command.WorkingDays)
        {
            var workingDayResult = TechnicianWorkingDay.Create(
                workingDay.Day,
                workingDay.StartTime,
                workingDay.EndTime);

            if (workingDayResult.IsError)
            {
                return workingDayResult.Errors;
            }

            workingDays.Add(workingDayResult.Value);
        }

        var settingsResult = TechnicianAvailabilitySettings.Create(
            command.Status,
            workingDays,
            command.VacationStartDate,
            command.VacationEndDate);

        if (settingsResult.IsError)
        {
            return settingsResult.Errors;
        }

        var configureResult = technicianProfile.ConfigureAvailability(
            settingsResult.Value);

        if (configureResult.IsError)
        {
            return configureResult.Errors;
        }

        technicianProfileRepository.Update(technicianProfile);

        return technicianProfile.ToTechnicianAvailabilitySettingsResponse();
    }
}
