namespace FixNow.Contracts.Responses;

public sealed record UpdateCurrentUserResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string? Email,
    string PhoneNumber);
