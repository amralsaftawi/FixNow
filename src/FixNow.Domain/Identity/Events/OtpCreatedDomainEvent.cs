

public sealed record OtpCreatedDomainEvent(
    Guid OtpRecordId,
    Guid UserId,
    OtpPurpose Purpose) : DomainEvent;