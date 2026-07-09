
public sealed record OtpVerifiedDomainEvent(
    Guid OtpRecordId,
    Guid UserId,
    OtpPurpose Purpose) : DomainEvent;