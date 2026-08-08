using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.Identity.Commands.ResetPassword;

public sealed record ResetPasswordCommand(
    string Identifier,
    string Otp,
    string NewPassword,
    string ConfirmPassword)
    : ICommand<Result<ResetPasswordResponse>>;