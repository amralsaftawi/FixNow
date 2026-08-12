namespace FixNow.Contracts.Responses;

public sealed record TechnicianAccountStatusResponse(
    Guid TechnicianProfileId,
    Guid UserId,
    AccountStatus Status);
