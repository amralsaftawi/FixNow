
public static class TechnicianProfileErrors
{
    public static readonly Error IdRequired =
        Error.Validation(
            "TechnicianProfile.IdRequired",
            "Technician profile id is required.");

    public static readonly Error UserIdRequired =
        Error.Validation(
            "TechnicianProfile.UserIdRequired",
            "User id is required.");

    public static readonly Error InvalidYearsOfExperience =
        Error.Validation(
            "TechnicianProfile.InvalidYearsOfExperience",
            "Years of experience cannot be negative.");

    public static readonly Error BioRequired =
        Error.Validation(
            "TechnicianProfile.BioRequired",
            "Technician bio is required.");

    public static readonly Error BioTooLong =
        Error.Validation(
            "TechnicianProfile.BioTooLong",
            "Technician bio cannot exceed 1000 characters.");

    public static readonly Error NationalIdImageRequired =
        Error.Validation(
            "TechnicianProfile.NationalIdImageRequired",
            "National ID image is required.");

    public static readonly Error AlreadyVerified =
        Error.Conflict(
            "TechnicianProfile.AlreadyVerified",
            "Technician profile is already verified.");

    public static readonly Error AlreadyRejected =
        Error.Conflict(
            "TechnicianProfile.AlreadyRejected",
            "Technician profile is already rejected.");

    public static readonly Error VerificationAlreadyPending =
        Error.Conflict(
            "TechnicianProfile.VerificationAlreadyPending",
            "Technician verification is already pending review.");

    public static readonly Error SameBio =
        Error.Conflict(
            "TechnicianProfile.SameBio",
            "The new bio is the same as the current bio.");
    
    public static readonly Error SameYearsOfExperience =
        Error.Conflict(
            "TechnicianProfile.SameYearsOfExperience",
            "The new Same Years Of Experience is the same as the current .");
    public static readonly Error SameAvailability =
        Error.Conflict(
            "TechnicianProfile.SameAvailability",
            "The technician already has this availability status.");

    public static readonly Error SameAvailabilitySettings =
        Error.Conflict(
            "TechnicianProfile.SameAvailabilitySettings",
            "The technician already has this availability configuration.");

    public static readonly Error SameNationalIdImage =
        Error.Conflict(
            "TechnicianProfile.SameNationalIdImage",
            "The national ID image is already the same.");

    public static readonly Error ProfileAlreadyCompleted =
        Error.Conflict(
            "TechnicianProfile.ProfileAlreadyCompleted",
            "The technician profile is already completed.");

    public static readonly Error AtLeastOneServiceRequired =
        Error.Validation(
            "TechnicianProfile.AtLeastOneServiceRequired",
            "At least one service must be added before completing the profile.");

    public static readonly Error ServiceAlreadyAdded =
        Error.Conflict(
            "TechnicianProfile.ServiceAlreadyAdded",
            "This service has already been added.");

    public static readonly Error ServiceNotFound =
        Error.NotFound(
            "TechnicianProfile.ServiceNotFound",
            "The requested service was not found.");

    public static readonly Error ServiceCategoryNotProvided =
        Error.Conflict(
            "TechnicianProfile.ServiceCategoryNotProvided",
            "The technician does not provide the requested service category.");

    public static readonly Error ExperienceRequired =
        Error.Validation(
            "TechnicianProfile.ExperienceRequired",
            "Technician experience is required.");

    public static readonly Error ExperienceAlreadyAdded =
        Error.Conflict(
            "TechnicianProfile.ExperienceAlreadyAdded",
            "This experience has already been added.");

    public static readonly Error ExperienceNotFound =
        Error.NotFound(
            "TechnicianProfile.ExperienceNotFound",
            "The requested experience was not found.");

    public static readonly Error PortfolioItemRequired =
        Error.Validation(
            "TechnicianProfile.PortfolioItemRequired",
            "Technician portfolio item is required.");

    public static readonly Error PortfolioItemAlreadyAdded =
        Error.Conflict(
            "TechnicianProfile.PortfolioItemAlreadyAdded",
            "This portfolio item has already been added.");

    public static readonly Error PortfolioItemNotFound =
        Error.NotFound(
            "TechnicianProfile.PortfolioItemNotFound",
            "The requested portfolio item was not found.");

    public static readonly Error LatitudeInvalid =
        Error.Validation(
            "TechnicianProfile.LatitudeInvalid",
            "Latitude must be between -90 and 90.");

    public static readonly Error LongitudeInvalid =
        Error.Validation(
            "TechnicianProfile.LongitudeInvalid",
            "Longitude must be between -180 and 180.");
}