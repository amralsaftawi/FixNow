
public static class UserSessionErrors
{
    public static readonly Error IdRequired =
        Error.Validation(
            "UserSession.IdRequired",
            "Session id is required.");

    public static readonly Error UserIdRequired =
        Error.Validation(
            "UserSession.UserIdRequired",
            "User id is required.");

    public static readonly Error RefreshTokenIdRequired =
        Error.Validation(
            "UserSession.RefreshTokenIdRequired",
            "Refresh token id is required.");

    public static readonly Error DeviceNameRequired =
        Error.Validation(
            "UserSession.DeviceNameRequired",
            "Device name is required.");

    public static readonly Error IpAddressRequired =
        Error.Validation(
            "UserSession.IpAddressRequired",
            "IP address is required.");

    public static readonly Error UserAgentRequired =
        Error.Validation(
            "UserSession.UserAgentRequired",
            "User agent is required.");

    public static readonly Error InvalidExpirationDate =
        Error.Validation(
            "UserSession.InvalidExpirationDate",
            "Expiration date must be in the future.");

    public static readonly Error AlreadyEnded =
        Error.Conflict(
            "UserSession.AlreadyEnded",
            "The session has already ended.");
}