namespace FixNow.Contracts.Responses;

public sealed record TechnicianVerificationStatusResponse(
    Guid TechnicianProfileId,
    VerificationStatus Status);
