using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.Identity.Commands.ResendOtp;

public sealed record ResendOtpCommand(
    string Identifier,
    string Purpose)
    : ICommand<Result<ResendOtpResponse>>;