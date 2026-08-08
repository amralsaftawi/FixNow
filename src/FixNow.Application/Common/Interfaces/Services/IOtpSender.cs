
namespace FixNow.Application.Common.Interfaces.Authentication;

public interface IOtpSender
{
    Task<Result<Success>> SendAsync(
        User user,
        string otp,
        OtpPurpose purpose,
        CancellationToken cancellationToken);
}