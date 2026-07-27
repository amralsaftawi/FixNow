namespace FixNow.Application.Features.Identity.Dtos.Shared;

public sealed record TokenUserInfo(
    Guid UserId,
    string Email,
    IReadOnlyCollection<string> Roles);