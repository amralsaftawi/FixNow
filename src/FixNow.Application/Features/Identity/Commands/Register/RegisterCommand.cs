
using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.Identity.Commands.Register;
public sealed record RegisterCommand(
    string FirstName,
    string LastName,
    string? Email,
    string PhoneNumber,
    string Password,
    string ConfirmPassword,
    string CountryCode,
    PreferredLanguage PreferredLanguage)
    : ICommand<Result<RegisterResponse>>;
 