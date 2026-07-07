public sealed record UserPhoneVerifiedDomainEvent(
    Guid UserId
) : DomainEvent;