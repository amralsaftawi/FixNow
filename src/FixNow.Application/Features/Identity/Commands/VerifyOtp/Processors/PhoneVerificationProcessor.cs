using FixNow.Application.Common.Interfaces.Persistence.Repositories;

namespace FixNow.Application.Features.Identity.Commands.VerifyOtp.Processors;

public sealed class PhoneVerificationProcessor(IUserRepository userRepository) : IOtpPurposeProcessor
{
    public OtpPurpose Purpose => OtpPurpose.PhoneVerification;

    public async Task<Result<Success>> ProcessAsync(
        User user,
        CancellationToken cancellationToken)
    {
        var result = user.VerifyPhone();

        if (result.IsError)
            return result.Errors;

        userRepository.Update(user);

        return await Task.FromResult(Result.Success);
    }
}
