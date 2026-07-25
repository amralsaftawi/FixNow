using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Domain.Common.Errors;

namespace FixNow.Application.Features.Identity.Commands.Login;

public sealed class LoginCommandHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    ITokenProvider tokenProvider)
    : ICommandHandler<LoginCommand, Result<LoginResponse>>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly ITokenProvider _tokenProvider = tokenProvider;

    public async Task<Result<LoginResponse>> Handle(
        LoginCommand command,
        CancellationToken cancellationToken)
    {
        var loginInfo = command.Login.Contains('@')
            ? await LoginByEmailAsync(
                command.Login,
                cancellationToken)
            : await LoginByPhoneNumberAsync(
                command.Login,
                cancellationToken);

        if (loginInfo is null)
        {
            return IdentityErrors.InvalidCredentials;
        }

        if (!_passwordHasher.Verify(
                command.Password,
                loginInfo.User.PasswordHash.Value))
        {
            return IdentityErrors.InvalidCredentials;
        }

        var tokenUserInfo = new TokenUserInfo(
            UserId: loginInfo.User.Id,
            Email: loginInfo.User.Email.Value,
            Roles: loginInfo.Roles);

        var accessTokenResult = await _tokenProvider.GenerateAsync(
            tokenUserInfo,
            cancellationToken);

        return loginInfo.User.ToLoginResponse(
            accessTokenResult);
    }

    private async Task<UserLoginInfo?> LoginByEmailAsync(
        string login,
        CancellationToken cancellationToken)
    {
        var emailResult = Email.Create(login);

        if (emailResult.IsError)
        {
            return null;
        }

        return await _userRepository.GetLoginInfoByEmailAsync(
            emailResult.Value,
            cancellationToken);
    }

    private async Task<UserLoginInfo?> LoginByPhoneNumberAsync(
        string login,
        CancellationToken cancellationToken)
    {
        var phoneNumberResult = PhoneNumber.Create(login);

        if (phoneNumberResult.IsError)
        {
            return null;
        }

        return await _userRepository.GetLoginInfoByPhoneNumberAsync(
            phoneNumberResult.Value,
            cancellationToken);
    }
}