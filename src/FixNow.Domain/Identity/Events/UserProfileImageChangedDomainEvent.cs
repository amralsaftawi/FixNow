public sealed record UserProfileImageChangedDomainEvent(
    Guid UserId,
    string? ProfileImageKey
) : DomainEvent;