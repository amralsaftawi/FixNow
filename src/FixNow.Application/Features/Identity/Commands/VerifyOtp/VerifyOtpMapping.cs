namespace FixNow.Application.Features.Identity.Commands.VerifyOtp;

public static class VerifyOtpMapping
{
    public static VerifyOtpResponse ToVerifyOtpResponse(string message = "OTP verified successfully.")
        => new(message);
}
