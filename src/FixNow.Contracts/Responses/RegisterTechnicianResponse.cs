namespace FixNow.Contracts.Responses;

public sealed record RegisterTechnicianResponse(
    Guid TechnicianProfileId,
    Guid UserId,
    VerificationStatus VerificationStatus,
    TechnicianAvailability Availability,
    int YearsOfExperience,
    string? Bio,
    string? NationalIdImageKey,
    bool IsProfileCompleted);
