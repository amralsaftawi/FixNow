namespace FixNow.Application.Features.Identity.Queries.GetCurrentUser;

public sealed record GetCurrentUserResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string? Email,
    string PhoneNumber);