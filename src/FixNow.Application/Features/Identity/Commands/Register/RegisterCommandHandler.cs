

using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Application.Features.Identity.Commands.Register;
using FixNow.Domain.Common.Errors;

public sealed class RegisterCommandHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher)
    : ICommandHandler<RegisterCommand, Result<RegisterResponse>>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;

    public async Task<Result<RegisterResponse>> Handle(
        RegisterCommand command,
        CancellationToken cancellationToken)
    {
        if (await _userRepository.ExistsByEmailAsync(
                command.Email,
                cancellationToken))
        {
            return IdentityErrors.EmailAlreadyExists;
        }

        if (await _userRepository.ExistsByPhoneNumberAsync(
                command.PhoneNumber,
                cancellationToken))
        {
            return IdentityErrors.PhoneNumberAlreadyExists;
        }

        var emailResult = Email.Create(command.Email);

        if (emailResult.IsError)
        {
            return emailResult.Errors;
        }

        var phoneNumberResult = PhoneNumber.Create(command.PhoneNumber);

        if (phoneNumberResult.IsError)
        {
            return phoneNumberResult.Errors;
        }

        var hashedPassword = _passwordHasher.Hash(command.Password);

        var passwordHashResult = PasswordHash.Create(hashedPassword);

        if (passwordHashResult.IsError)
        {
            return passwordHashResult.Errors;
        }

        var createUserResult = User.Create(
            id: Guid.NewGuid(),
            firstName: command.FirstName,
            lastName: command.LastName,
            email: emailResult.Value,
            phoneNumber: phoneNumberResult.Value,
            passwordHash: passwordHashResult.Value,
            countryCode: command.CountryCode,
            preferredLanguage: command.PreferredLanguage,
            registeredVia: AuthProvider.Phone);

        if (createUserResult.IsError)
        {
            return createUserResult.Errors;
        }

        var user = createUserResult.Value;

        await _userRepository.AddAsync(
            user,
            cancellationToken);

        return user.ToRegisterResponse();
    }
}