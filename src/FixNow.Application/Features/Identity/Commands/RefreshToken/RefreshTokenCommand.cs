using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.Identity.Commands.RefreshToken;

public sealed record RefreshTokenCommand(
    string RefreshToken)
    : ICommand<Result<RefreshTokenResponse>>;