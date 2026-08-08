namespace FixNow.Application.Features.Identity.Commands.ResendOtp;

public static class ResendOtpMapping
{
    public static ResendOtpResponse ToResendOtpResponse(
        string message)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(message);

        return new ResendOtpResponse(message);
    }
}