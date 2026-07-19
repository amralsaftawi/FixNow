namespace FixNow.Application.Features.Identity.Commands.Register;

public sealed record RegisterResponse(
    Guid UserId,
    string Email);