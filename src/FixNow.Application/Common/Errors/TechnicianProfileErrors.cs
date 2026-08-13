public static class TechnicianProfileErrors
{
    

     public static readonly Error AlreadyExists =
        Error.Conflict(
            code: "TechnicianProfile.AlreadyExists",
            description: "The Technician Profile is Already Exists.");

            


            
     public static readonly Error  NotFound=
        Error.NotFound(
            code: "TechnicianProfile.NotFound",
            description: "The Technician Profile is Not Found.");

    public static readonly Error NationalIdImageOwnershipInvalid =
        Error.Validation(
            code: "TechnicianProfile.NationalIdImageKey.OwnershipInvalid",
            description: "The national ID image must belong to the current user.");

    public static readonly Error NationalIdImageRequired =
        Error.Validation(
            code: "TechnicianProfile.NationalIdImageRequired",
            description: "National ID image is required.");

    public static readonly Error ExperienceNotFound =
        Error.NotFound(
            code: "TechnicianProfile.ExperienceNotFound",
            description: "The requested experience was not found.");

    public static readonly Error ServiceNotFound =
        Error.NotFound(
            code: "TechnicianProfile.ServiceNotFound",
            description: "The requested service was not found.");

    public static readonly Error ServiceCategoryNotProvided =
        Error.Conflict(
            code: "TechnicianProfile.ServiceCategoryNotProvided",
            description: "The technician does not provide the requested service category.");

    public static readonly Error PortfolioItemNotFound =
        Error.NotFound(
            code: "TechnicianProfile.PortfolioItemNotFound",
            description: "The requested portfolio item was not found.");

    public static readonly Error PortfolioMediaOwnershipInvalid =
        Error.Validation(
            code: "TechnicianProfile.PortfolioMediaKey.OwnershipInvalid",
            description: "The portfolio media must belong to the current user.");


}