public interface ISmsOtpSender
{
    Task<Result<Success>> SendAsync(
        string phoneNumber,
        string otp,
        CancellationToken cancellationToken);
}
