using FixNow.Application.Common.Models;
using FixNow.Application.Features.Identity.Commands.Register;

public sealed record RegisterCommand(
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    string Password,
    string ConfirmPassword)
    : ICommand<Result<RegisterResponse>>;