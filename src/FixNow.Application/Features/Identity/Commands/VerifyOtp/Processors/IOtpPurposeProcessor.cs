namespace FixNow.Application.Features.Identity.Commands.VerifyOtp.Processors;

public interface IOtpPurposeProcessor
{
    OtpPurpose Purpose { get; }

    Task<Result<Success>> ProcessAsync(
        User user,
        CancellationToken cancellationToken);
}
