
public static class TechnicianServiceErrors
{
    public static readonly Error IdRequired =
        Error.Validation(
            "TechnicianService.IdRequired",
            "Technician service id is required.");

    public static readonly Error TechnicianProfileIdRequired =
        Error.Validation(
            "TechnicianService.TechnicianProfileIdRequired",
            "Technician profile id is required.");

    public static readonly Error ServiceCategoryIdRequired =
        Error.Validation(
            "TechnicianService.ServiceCategoryIdRequired",
            "Service category id is required.");
}