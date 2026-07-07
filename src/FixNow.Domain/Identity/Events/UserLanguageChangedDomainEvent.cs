public sealed record UserLanguageChangedDomainEvent(
    Guid UserId,
    PreferredLanguage NewLanguage
) : DomainEvent;