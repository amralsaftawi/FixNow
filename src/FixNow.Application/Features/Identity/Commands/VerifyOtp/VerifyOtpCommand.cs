using FixNow.Application.Common.Abstractions.Messaging;

namespace FixNow.Application.Features.Identity.Commands.VerifyOtp;

public sealed record VerifyOtpCommand(
    string Identifier,
    string Otp,
    string Purpose) : ICommand<Result<VerifyOtpResponse>>;
