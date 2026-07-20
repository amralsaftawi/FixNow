using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.Identity.Commands.Login;

public sealed record LoginCommand(
    string Login,
    string Password)
    : ICommand<Result<LoginResponse>>;