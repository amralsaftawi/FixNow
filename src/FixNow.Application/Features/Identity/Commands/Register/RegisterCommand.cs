
using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Features.Identity.Commands.Register;

public sealed record RegisterCommand(
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    string Password,
    string ConfirmPassword,
    CountryCode CountryCode,
    PreferredLanguage PreferredLanguage)
    : ICommand<Result<RegisterResponse>>;