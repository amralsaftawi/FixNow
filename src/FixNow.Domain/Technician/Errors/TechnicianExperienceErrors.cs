
public static class TechnicianExperienceErrors
{
    public static readonly Error IdRequired =
        Error.Validation(
            "TechnicianExperience.IdRequired",
            "Technician experience id is required.");

    public static readonly Error TechnicianProfileIdRequired =
        Error.Validation(
            "TechnicianExperience.TechnicianProfileIdRequired",
            "Technician profile id is required.");

    public static readonly Error CompanyNameRequired =
        Error.Validation(
            "TechnicianExperience.CompanyNameRequired",
            "Company name is required.");

    public static readonly Error CompanyNameTooLong =
        Error.Validation(
            "TechnicianExperience.CompanyNameTooLong",
            "Company name cannot exceed 150 characters.");

    public static readonly Error PositionRequired =
        Error.Validation(
            "TechnicianExperience.PositionRequired",
            "Position is required.");

    public static readonly Error PositionTooLong =
        Error.Validation(
            "TechnicianExperience.PositionTooLong",
            "Position cannot exceed 150 characters.");

    public static readonly Error DescriptionTooLong =
        Error.Validation(
            "TechnicianExperience.DescriptionTooLong",
            "Description cannot exceed 1000 characters.");

    public static readonly Error StartDateRequired =
        Error.Validation(
            "TechnicianExperience.StartDateRequired",
            "Start date is required.");

    public static readonly Error EndDateBeforeStartDate =
        Error.Validation(
            "TechnicianExperience.EndDateBeforeStartDate",
            "End date cannot be before or equal to the start date.");
}
