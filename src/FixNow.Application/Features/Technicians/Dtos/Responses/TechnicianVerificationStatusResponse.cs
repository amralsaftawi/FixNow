namespace FixNow.Application.Features.TechnicianProfiles.Dtos.Responses;

public sealed record TechnicianVerificationStatusResponse(
    Guid TechnicianProfileId,
    VerificationStatus Status);
