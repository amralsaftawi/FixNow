using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.Identity.Commands.Logout;

public sealed record LogoutCommand(
    string RefreshToken)
    : ICommand<Result<Success>>;