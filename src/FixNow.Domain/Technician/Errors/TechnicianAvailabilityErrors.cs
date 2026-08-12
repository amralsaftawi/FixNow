
public static class TechnicianAvailabilityErrors
{
    public static readonly Error AvailabilitySettingsRequired =
        Error.Validation(
            "TechnicianAvailability.SettingsRequired",
            "Technician availability settings are required.");

    public static readonly Error InvalidStatus =
        Error.Validation(
            "TechnicianAvailability.Status.Invalid",
            "The specified availability status is invalid.");

    public static readonly Error WorkingDayRequired =
        Error.Validation(
            "TechnicianAvailability.WorkingDay.Required",
            "At least one working day collection must be provided.");

    public static readonly Error InvalidWorkingDay =
        Error.Validation(
            "TechnicianAvailability.WorkingDay.InvalidDay",
            "The specified working day is invalid.");

    public static readonly Error EndTimeNotAfterStartTime =
        Error.Validation(
            "TechnicianAvailability.WorkingDay.EndTimeNotAfterStartTime",
            "Working day end time must be after start time.");

    public static readonly Error DuplicateWorkingDay =
        Error.Validation(
            "TechnicianAvailability.DuplicateWorkingDay",
            "The same working day cannot be configured more than once.");

    public static readonly Error VacationDatesBothRequired =
        Error.Validation(
            "TechnicianAvailability.VacationDatesBothRequired",
            "Both vacation start and end dates must be provided together.");

    public static readonly Error InvalidVacationRange =
        Error.Validation(
            "TechnicianAvailability.InvalidVacationRange",
            "Vacation end date must not be before vacation start date.");
}
