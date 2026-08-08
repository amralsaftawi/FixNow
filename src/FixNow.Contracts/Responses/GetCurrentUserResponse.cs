namespace FixNow.Contracts.Responses;

public sealed record GetCurrentUserResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string? Email,
    string PhoneNumber);