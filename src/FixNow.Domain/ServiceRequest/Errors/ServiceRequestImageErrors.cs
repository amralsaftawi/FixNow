
public static class ServiceRequestImageErrors
{
    public static readonly Error IdRequired =
        Error.Validation(
            "ServiceRequestImage.IdRequired",
            "Service request image id is required.");

    public static readonly Error ServiceRequestIdRequired =
        Error.Validation(
            "ServiceRequestImage.ServiceRequestIdRequired",
            "Service request id is required.");

    public static readonly Error ImageKeyRequired =
        Error.Validation(
            "ServiceRequestImage.ImageKeyRequired",
            "Image key is required.");
}