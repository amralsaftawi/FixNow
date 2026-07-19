public sealed record TokenUserInfo(
    Guid UserId,
    string Email,
    IReadOnlyCollection<string> Roles);