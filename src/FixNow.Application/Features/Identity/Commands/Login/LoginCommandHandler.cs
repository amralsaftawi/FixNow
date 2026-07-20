
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
        var user = command.Login.Contains('@')
            ? await LoginByEmailAsync(command.Login, cancellationToken)
            : await LoginByPhoneNumberAsync(command.Login, cancellationToken);

        if (user is null)
        {
            return IdentityErrors.InvalidCredentials;
        }

        if (!_passwordHasher.Verify(
                command.Password,
                user.PasswordHash.Value))
        {
            return IdentityErrors.InvalidCredentials;
        }

       var tokenUserInfo = new TokenUserInfo(
    UserId: user.Id,
    Email: user.Email.Value,
    Roles: user.Roles.Select(r => r.Name).ToList());

var accessTokenResult = await _tokenProvider.GenerateAsync(
    tokenUserInfo,
    cancellationToken);

return user.ToLoginResponse(accessTokenResult);
    }

    private async Task<User?> LoginByEmailAsync(
        string login,
        CancellationToken cancellationToken)
    {
        var emailResult = Email.Create(login);

        if (emailResult.IsError)
        {
            return null;
        }

        return await _userRepository.GetByEmailAsync(
            emailResult.Value,
            cancellationToken);
    }

    private async Task<User?> LoginByPhoneNumberAsync(
        string login,
        CancellationToken cancellationToken)
    {
        var phoneResult = PhoneNumber.Create(login);

        if (phoneResult.IsError)
        {
            return null;
        }

        return await _userRepository.GetByPhoneNumberAsync(
            phoneResult.Value,
            cancellationToken);
    }
}