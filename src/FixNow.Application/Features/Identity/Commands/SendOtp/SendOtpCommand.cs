using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.Identity.Commands.SendOtp;

public sealed record SendOtpCommand(string Identifier)
    : ICommand<Result<SendOtpResponse>>;
