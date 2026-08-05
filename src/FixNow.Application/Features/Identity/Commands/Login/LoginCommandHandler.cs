using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Domain.Common.Errors;

namespace FixNow.Application.Features.Identity.Commands.Login;

public sealed class LoginCommandHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    ITokenService tokenService,
    IRefreshTokenService refreshTokenService)
    : ICommandHandler<LoginCommand, Result<LoginResponse>>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly ITokenService _tokenService = tokenService;
    private readonly IRefreshTokenService _refreshTokenService = refreshTokenService;

    public async Task<Result<LoginResponse>> Handle(
        LoginCommand command,
        CancellationToken cancellationToken)
    {
        // 1. Find user by email or phone number
        var userResult = await FindUserAsync(
            command.Identifier,
            cancellationToken);

        if (userResult.IsError)
        {
            return userResult.Errors;
        }

        var user = userResult.Value;

        // 2. Check account status
        if (user.AccountStatus != AccountStatus.Active)
        {
            return IdentityErrors.AccountNotActive;
        }

        // 3. Verify password
        var isPasswordValid = _passwordHasher.Verify(
            command.Password,
            user.PasswordHash);

        if (!isPasswordValid)
        {
            return IdentityErrors.InvalidCredentials;
        }

        // 4. Generate access token
        var accessTokenResult = _tokenService.GenerateAccessToken(user);

        if (accessTokenResult.IsError)
        {
            return accessTokenResult.Errors;
        }

        // 5. Generate refresh token
        var refreshTokenResult =
            _refreshTokenService.Generate();

        if (refreshTokenResult.IsError)
        {
            return refreshTokenResult.Errors;
        }

        var refreshToken = refreshTokenResult.Value;

        // 6. Persist refresh token/session
        var storeRefreshTokenResult = await _refreshTokenService.StoreAsync(
            user.Id,
            refreshToken,
            cancellationToken);

        if (storeRefreshTokenResult.IsError)
            return storeRefreshTokenResult.Errors;

        // 7. Return response
        return new LoginResponse(
            AccessToken: accessTokenResult.Value.Token,
            AccessTokenExpiresAt: accessTokenResult.Value.ExpiresAt,
            RefreshToken: refreshToken.Token,
            RefreshTokenExpiresAt: refreshToken.ExpiresAt);
    }

    private async Task<Result<User>> FindUserAsync(
        string identifier,
        CancellationToken cancellationToken)
    {
        var emailResult = Email.Create(identifier);

        if (!emailResult.IsError)
        {
            var userByEmail = await _userRepository.GetByEmailAsync( emailResult.Value,cancellationToken);

            if (userByEmail is not null)
            {
                return userByEmail;
            }
        }

        var phoneResult = PhoneNumber.Create(identifier);

        if (!phoneResult.IsError)
        {
            var userByPhone = await _userRepository
                .GetByPhoneNumberAsync(
                    phoneResult.Value,
                    cancellationToken);

            if (userByPhone is not null)
            {
                return userByPhone;
            }
        }

        return IdentityErrors.InvalidCredentials;
    }
}
