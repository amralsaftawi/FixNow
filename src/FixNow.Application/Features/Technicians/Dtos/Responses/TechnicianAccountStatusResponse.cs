namespace FixNow.Application.Features.TechnicianProfiles.Dtos.Responses;

public sealed record TechnicianAccountStatusResponse(
    Guid TechnicianProfileId,
    Guid UserId,
    AccountStatus Status);
