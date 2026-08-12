
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

    public static readonly Error PriceRequired =
        Error.Validation(
            "TechnicianService.PriceRequired",
            "Technician service price is required.");

    public static readonly Error SamePrice =
        Error.Conflict(
            "TechnicianService.SamePrice",
            "The new price is the same as the current price.");
}