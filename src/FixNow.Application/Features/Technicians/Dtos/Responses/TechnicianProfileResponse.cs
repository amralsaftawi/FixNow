namespace FixNow.Application.Features.TechnicianProfiles.Dtos.Responses;

public sealed record TechnicianProfileResponse(
    Guid TechnicianProfileId,
    Guid UserId,
    TechnicianAvailability Availability,
    int YearsOfExperience,
    string? Bio,
    string? NationalIdImageKey,
    bool IsProfileCompleted,
    VerificationStatus VerificationStatus);