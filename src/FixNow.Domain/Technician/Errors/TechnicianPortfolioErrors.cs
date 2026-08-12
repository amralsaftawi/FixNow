
public static class TechnicianPortfolioErrors
{
    public static readonly Error IdRequired =
        Error.Validation(
            "TechnicianPortfolioItem.IdRequired",
            "Portfolio item id is required.");

    public static readonly Error TechnicianProfileIdRequired =
        Error.Validation(
            "TechnicianPortfolioItem.TechnicianProfileIdRequired",
            "Technician profile id is required.");

    public static readonly Error TitleRequired =
        Error.Validation(
            "TechnicianPortfolioItem.TitleRequired",
            "Portfolio item title is required.");

    public static readonly Error TitleTooLong =
        Error.Validation(
            "TechnicianPortfolioItem.TitleTooLong",
            "Portfolio item title cannot exceed 150 characters.");

    public static readonly Error DescriptionTooLong =
        Error.Validation(
            "TechnicianPortfolioItem.DescriptionTooLong",
            "Portfolio item description cannot exceed 1000 characters.");

    public static readonly Error MediaKeyRequired =
        Error.Validation(
            "TechnicianPortfolioItem.MediaKeyRequired",
            "Portfolio media keys cannot be empty.");

    public static readonly Error MediaKeyTooLong =
        Error.Validation(
            "TechnicianPortfolioItem.MediaKeyTooLong",
            "Portfolio media key cannot exceed 500 characters.");

    public static readonly Error DuplicateMediaKey =
        Error.Conflict(
            "TechnicianPortfolioItem.DuplicateMediaKey",
            "The same portfolio media key cannot be added more than once.");

    public static readonly Error MediaIdRequired =
        Error.Validation(
            "TechnicianPortfolioMedia.IdRequired",
            "Portfolio media id is required.");

    public static readonly Error TechnicianPortfolioItemIdRequired =
        Error.Validation(
            "TechnicianPortfolioMedia.TechnicianPortfolioItemIdRequired",
            "Portfolio item id is required.");

    public static readonly Error InvalidDisplayOrder =
        Error.Validation(
            "TechnicianPortfolioMedia.InvalidDisplayOrder",
            "Portfolio media display order cannot be negative.");

    public static readonly Error PortfolioItemNotFound =
        Error.NotFound(
            "TechnicianPortfolioItem.NotFound",
            "The requested portfolio item was not found.");
}
