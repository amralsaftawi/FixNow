
public static class ServiceRequestErrors
{
    public static readonly Error IdRequired =
        Error.Validation(
            "ServiceRequest.IdRequired",
            "Service request id is required.");

    public static readonly Error CustomerProfileIdRequired =
        Error.Validation(
            "ServiceRequest.CustomerProfileIdRequired",
            "Customer profile id is required.");

    public static readonly Error AddressIdRequired =
        Error.Validation(
            "ServiceRequest.AddressIdRequired",
            "Address id is required.");

    public static readonly Error ServiceCategoryIdRequired =
        Error.Validation(
            "ServiceRequest.ServiceCategoryIdRequired",
            "Service category id is required.");

    public static readonly Error DescriptionRequired =
        Error.Validation(
            "ServiceRequest.DescriptionRequired",
            "Description is required.");

    public static readonly Error DescriptionTooLong =
        Error.Validation(
            "ServiceRequest.DescriptionTooLong",
            "Description cannot exceed 2000 characters.");

    public static readonly Error InvalidScheduleDate =
        Error.Validation(
            "ServiceRequest.InvalidScheduleDate",
            "Scheduled date must be in the future.");

    public static readonly Error SameScheduleDate =
        Error.Conflict(
            "ServiceRequest.SameScheduleDate",
            "The service request already has this preferred service time.");

    public static readonly Error NotFound =
        Error.NotFound(
            "ServiceRequest.NotFound",
            "The service request was not found.");

    public static readonly Error SameCategory =
        Error.Conflict(
            "ServiceRequest.SameCategory",
            "The service request already has this category.");

    public static readonly Error ProblemTypeIdRequired =
        Error.Validation(
            "ServiceRequest.ProblemTypeIdRequired",
            "Problem type id is required.");

    public static readonly Error SameProblemType =
        Error.Conflict(
            "ServiceRequest.SameProblemType",
            "The service request already has this problem type.");

    public static readonly Error ProblemTypeIncompatible =
        Error.Conflict(
            "ServiceRequest.ProblemTypeIncompatible",
            "The selected problem type is not valid for the service request's category.");

    public static readonly Error SameDescription =
        Error.Conflict(
            "ServiceRequest.SameDescription",
            "The new description is the same as the current one.");

    public static readonly Error SamePriority =
        Error.Conflict(
            "ServiceRequest.SamePriority",
            "The service request already has this priority.");

    public static readonly Error InvalidStatusTransition =
        Error.Conflict(
            "ServiceRequest.InvalidStatusTransition",
            "The requested status transition is not allowed.");

    public static readonly Error AlreadyCompleted =
        Error.Conflict(
            "ServiceRequest.AlreadyCompleted",
            "The service request has already been completed.");

    public static readonly Error AlreadyAccepted =
        Error.Conflict(
            "ServiceRequest.AlreadyAccepted",
            "The service request has already been accepted.");

    public static readonly Error AlreadyInProgress =
        Error.Conflict(
            "ServiceRequest.AlreadyInProgress",
            "The service request is already in progress.");

    public static readonly Error AlreadyCancelled =
        Error.Conflict(
            "ServiceRequest.AlreadyCancelled",
            "The service request has already been cancelled.");

    public static readonly Error ImageAlreadyAdded =
        Error.Conflict(
            "ServiceRequest.ImageAlreadyAdded",
            "This image has already been added.");

    public static readonly Error ImageNotFound =
        Error.NotFound(
            "ServiceRequest.ImageNotFound",
            "The requested image was not found.");

    public static readonly Error EstimatedCostRequired =
        Error.Validation(
            "ServiceRequest.EstimatedCostRequired",
            "Estimated cost is required.");

    public static readonly Error SameEstimatedCost =
        Error.Conflict(
            "ServiceRequest.SameEstimatedCost",
            "The service request already has this estimated cost.");
}