public interface IEmailOtpSender
{
    Task<Result<Success>> SendAsync(
        string email,
        string otp,
        CancellationToken cancellationToken);
}
