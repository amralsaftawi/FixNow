
namespace FixNow.Contracts.Responses;

public sealed record RegisterResponse(
    Guid UserId,
    string Message);