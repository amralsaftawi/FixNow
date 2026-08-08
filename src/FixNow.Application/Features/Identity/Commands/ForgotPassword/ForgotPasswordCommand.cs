using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.Identity.Commands.ForgotPassword;

public sealed record ForgotPasswordCommand(
    string Identifier)
    : ICommand<Result<ForgotPasswordResponse>>;