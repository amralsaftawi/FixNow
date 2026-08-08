using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.Identity.Commands.DeactivateCurrentUser;

public sealed record DeactivateCurrentUserCommand
    : ICommand<Result<Success>>;
