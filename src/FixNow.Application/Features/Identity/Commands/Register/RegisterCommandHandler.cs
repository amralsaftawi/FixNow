using FixNow.Application.Common.Abstractions.Messaging;
using FixNow.Application.Common.Interfaces.Persistence.Repositories;
using FixNow.Domain.Common.Errors;

namespace FixNow.Application.Features.Identity.Commands.Register;

public sealed class RegisterCommandHandler(IUserRepository userRepository,IPasswordHasher passwordHasher)
    : ICommandHandler<RegisterCommand, Result<RegisterResponse>>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;

    public async Task<Result<RegisterResponse>> Handle( RegisterCommand command, CancellationToken cancellationToken)
    {
        // 1. Create Email Value Object
        Email? email = null;

        if (!string.IsNullOrWhiteSpace(command.Email))
        {
            var emailResult = Email.Create(command.Email);

            if (emailResult.IsError)
                return emailResult.Errors;

            email = emailResult.Value;
        }

        // 2. Create PhoneNumber Value Object
        var phoneNumberResult = PhoneNumber.Create(command.PhoneNumber);

        if (phoneNumberResult.IsError)
            return phoneNumberResult.Errors;

        var phoneNumber = phoneNumberResult.Value;

        // 3. Create CountryCode Value Object
        var countryCodeResult = CountryCode.Create(command.CountryCode);

        if (countryCodeResult.IsError)
            return countryCodeResult.Errors;

        var countryCode = countryCodeResult.Value;

        // 4. Check Email uniqueness
        if (email is not null && await _userRepository.ExistsByEmailAsync(email, cancellationToken))
        {
            return IdentityErrors.EmailAlreadyExists;
        }

        // 5. Check Phone uniqueness
        if (await _userRepository.ExistsByPhoneNumberAsync(phoneNumber,cancellationToken))
        {
            return IdentityErrors.PhoneNumberAlreadyExists;
        }

        // 6. Hash Password
        var hashedPassword = _passwordHasher.Hash(command.Password);

        var passwordHashResult = PasswordHash.Create(hashedPassword);

        if (passwordHashResult.IsError)
            return passwordHashResult.Errors;

        // 7. Create User
        var createUserResult = User.Create(
            id: Guid.NewGuid(),
            firstName: command.FirstName,
            lastName: command.LastName,
            email: email,
            phoneNumber: phoneNumber,
            passwordHash: passwordHashResult.Value,
            countryCode: countryCode,
            preferredLanguage: command.PreferredLanguage,
            registeredVia: AuthProvider.Phone);

        if (createUserResult.IsError)
            return createUserResult.Errors;

        var user = createUserResult.Value;

        // 8. Persist User
        await _userRepository.AddAsync( user, cancellationToken);

        // 9. Return Response
        return user.ToRegisterResponse();
    }
}