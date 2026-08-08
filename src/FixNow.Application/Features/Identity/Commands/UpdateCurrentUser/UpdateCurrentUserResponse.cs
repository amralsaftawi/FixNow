namespace FixNow.Application.Features.Identity.Commands.UpdateCurrentUser;

public sealed record UpdateCurrentUserResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string? Email,
    string PhoneNumber);
