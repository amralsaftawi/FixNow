using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.Identity.Commands.ChangePassword;

public sealed record ChangePasswordCommand(
    string CurrentPassword,
    string NewPassword,
    string ConfirmPassword)
    : ICommand<Result<ChangePasswordResponse>>;
