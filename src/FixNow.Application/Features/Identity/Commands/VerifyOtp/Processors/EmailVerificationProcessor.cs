using FixNow.Application.Common.Interfaces.Persistence.Repositories;

namespace FixNow.Application.Features.Identity.Commands.VerifyOtp.Processors;

public sealed class EmailVerificationProcessor(IUserRepository userRepository) : IOtpPurposeProcessor
{
    public OtpPurpose Purpose => OtpPurpose.EmailVerification;

    public async Task<Result<Success>> ProcessAsync(
        User user,
        CancellationToken cancellationToken)
    {
        var result = user.VerifyEmail();

        if (result.IsError)
            return result.Errors;

        userRepository.Update(user);

        return await Task.FromResult(Result.Success);
    }
}
