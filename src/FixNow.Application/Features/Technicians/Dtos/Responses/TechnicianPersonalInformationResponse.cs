namespace FixNow.Application.Features.TechnicianProfiles.Dtos.Responses;

public sealed record TechnicianPersonalInformationResponse(
    Guid UserId,
    Guid TechnicianProfileId,
    string FirstName,
    string LastName,
    string? Email,
    string PhoneNumber,
    string CountryCode,
    PreferredLanguage PreferredLanguage,
    int YearsOfExperience,
    string? Bio,
    string? NationalIdImageKey,
    bool IsProfileCompleted,
    VerificationStatus VerificationStatus);
