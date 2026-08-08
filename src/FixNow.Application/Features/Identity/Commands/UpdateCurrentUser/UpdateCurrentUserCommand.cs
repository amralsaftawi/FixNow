using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.Identity.Commands.UpdateCurrentUser;

public sealed record UpdateCurrentUserCommand(
    string FirstName,
    string LastName,
    string? Email,
    string PhoneNumber,
    string CountryCode,
    PreferredLanguage PreferredLanguage)
    : ICommand<Result<UpdateCurrentUserResponse>>;
